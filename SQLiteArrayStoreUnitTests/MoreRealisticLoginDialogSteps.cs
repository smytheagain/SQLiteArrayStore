using NUnit.Framework;
using SQLiteArrayStore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TechTalk.SpecFlow;

namespace SQLiteArrayStoreUnitTests
{
    [Binding]
    [Scope(Feature = "MoreRealisticLoginDialog")]
    public class MoreRealisticLoginDialogSteps
    {
        private IAuthenticate _authenticationObject;

        private AuthenticationResult _authenticationResult;

        private Thread _loginWindowThread;

        private Window _loginWindowObject;

        [Given(@"a log in request")]
        public void GivenALogInRequest()
        {
            _authenticationObject = new Authenticate();

            _loginWindowThread = new Thread(new ThreadStart(() =>
            {
                _authenticationResult = _authenticationObject.AuthenticateUser();
            }));

            _loginWindowThread.SetApartmentState(ApartmentState.STA);
            _loginWindowThread.Start();

            _loginWindowObject = GetDialogWindowObjectFromHandle();
        }

        /// <summary>
        /// Attempts to get a window object from the main window handle of the current process.
        /// </summary>
        /// <returns>Window object associated with the current process.</returns>
        private static Window GetDialogWindowObjectFromHandle()
        {
            IntPtr mainWindowHandle = IntPtr.Zero;

            int escapeCount = 0;
            while (mainWindowHandle == IntPtr.Zero && escapeCount < 100)
            {
                mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
                Thread.Sleep(50);
                escapeCount++;
            }

            return (Window)System.Windows.Interop.HwndSource.FromHwnd(mainWindowHandle).RootVisual;
        }

        [When(@"'(.*)' and '(.*)' is entered into the login dialog")]
        public void WhenAndIsEnteredIntoTheLoginDialog(string userName, string password)
        {
            string userNameCopy = userName;
            string passwordCopy = password;

            DispatcherOperation operation = Dispatcher.FromThread(_loginWindowThread).BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                List<TextBox> textBoxes = FindVisualChildren<TextBox>(_loginWindowObject).ToList();
                TextBox userNameText = textBoxes.First(t => AutomationProperties.GetAutomationId(t) == SQLiteArrayStore.Resources.AutomationIds.LoginDialog_UsernameText);
                TextBox passwordText = textBoxes.First(t => AutomationProperties.GetAutomationId(t) == SQLiteArrayStore.Resources.AutomationIds.LoginDialog_PasswordText);

                userNameText.Clear();
                userNameText.Text = userNameCopy;
                userNameText.RaiseEvent(new RoutedEventArgs(TextBox.TextChangedEvent));

                passwordText.Clear();
                passwordText.Text = passwordCopy;
                passwordText.RaiseEvent(new RoutedEventArgs(TextBox.TextChangedEvent));
            }));

            operation.Wait(TimeSpan.FromSeconds(1)); // allow the UI to redraw
        }

        /// <summary>
        /// Finds the all visible child controls of a type within a wpf parent object.
        /// </summary>
        /// <typeparam name="T">The type of the child control to find.</typeparam>
        /// <param name="depObj">The parent wpf object.</param>
        /// <returns>IEnumerable of child obects of the specified type.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj != null)
            {
                // Can use LogicalTreeHelper for non-visual or not currently visible children
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        [When(@"the user accepts the login dialog")]
        public void WhenTheUserAcceptsTheLoginDialog()
        {
            Dispatcher.FromThread(_loginWindowThread).BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                List<Button> buttons = FindVisualChildren<Button>(_loginWindowObject).ToList();
                Button okButton = buttons.First(b => AutomationProperties.GetAutomationId(b) == SQLiteArrayStore.Resources.AutomationIds.LoginDialog_OKButton);
                okButton.Command.Execute(null);

                WaitForWindowToClose(_loginWindowObject);

                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }));

            _loginWindowThread.Join();
        }


        private static void WaitForWindowToClose(Window window)
        {
            int escapeCount = 0;
            int maxEscapeCount = 20;
            while (window.IsVisible && escapeCount < maxEscapeCount)
            {
                Thread.Sleep(500);
                escapeCount++;
            }

            Assert.Less(escapeCount, maxEscapeCount, "Failed to wait for window to unload.");
        }

        [Then(@"the authentication result is '(.*)'")]
        public void ThenTheAuthenticationResultIs(AuthenticationResult authResult)
        {
            Assert.AreEqual(authResult, _authenticationResult, "Authentication result check");
        }
    }
}
