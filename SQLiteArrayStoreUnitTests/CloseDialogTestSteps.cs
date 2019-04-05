using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteArrayStoreUnitTests
{
    [Binding]
    public class CloseDialogTestSteps
    {
        private Thread messageThread;
        private MessageBoxTestHelper messageBoxTestHelperInstance;
        private ISimpleMessageBoxAutomation messageWindowPO;
        private Window messageWindow;

        [AfterScenario]
        public void ScenarioTearDown()
        {
            if (this.messageThread.ThreadState != System.Threading.ThreadState.Stopped)
            {
                Dispatcher.FromThread(messageThread).InvokeShutdown();

                int escapeCount = 0;
                while (!Dispatcher.FromThread(messageThread).HasShutdownFinished && escapeCount < 10)
                {
                    Thread.Sleep(500);
                    escapeCount++;
                }
            }
        }

        [Given(@"I have the simple messagebox window open")]
        public void GivenIHaveTheSimpleMessageboxWindowOpen()
        {
            messageThread = CreateMessageBoxOnSeparateThread();
            messageThread.Start();
        }

        private Thread CreateMessageBoxOnSeparateThread()
        {
            Thread t = new Thread(new ThreadStart( () =>
            {
                this.messageBoxTestHelperInstance = new MessageBoxTestHelper();
                this.messageBoxTestHelperInstance.ShowMessageBox();

                Dispatcher.Run();
            }));

            t.SetApartmentState(ApartmentState.STA);

            return t;
        }
        
        [When(@"I invoke the close method")]
        public void WhenIInvokeTheCloseMethod()
        {
            Thread.Sleep(2000);

            Dispatcher.FromThread(messageThread).Invoke(DispatcherPriority.Background, new Action(() =>
            {
                this.messageBoxTestHelperInstance.MessageBoxInstance.Close();
            }));
        }
        
        [Then(@"the message box closes")]
        public void ThenTheMessageBoxCloses()
        {
            bool messageBoxVisible = Dispatcher.FromThread(messageThread).Invoke(new Func<bool>(() =>
            {
                return this.messageBoxTestHelperInstance.MessageBoxInstance.IsVisible;
            }), DispatcherPriority.Background);

            Assert.AreEqual(false, messageBoxVisible);
        }

        [When(@"I invoke the ok button click event")]
        public void WhenIInvokeTheOkButtonClickEvent()
        {
            Thread.Sleep(2000);

            Dispatcher.FromThread(messageThread).Invoke(DispatcherPriority.Background, new Action(() =>
            {
                this.messageBoxTestHelperInstance.MessageBoxInstance.okButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }));
        }

        [Given(@"The simple messagebox is opened without direct access to it")]
        public void GivenTheSimpleMessageboxIsOpenedWithoutDirectAccessToIt()
        {
            ShowMessageAsync();
        }

        [When(@"I invoke the ok button click event using the window handle")]
        public void WhenIInvokeTheOkButtonClickEventUsingTheWindowHandle()
        {
            Thread.Sleep(2000);

            var test = Process.GetCurrentProcess().MainWindowHandle;

            this.messageWindow = (Window)System.Windows.Interop.HwndSource.FromHwnd(test).RootVisual;

            this.messageWindow.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                FindVisualChildren<Button>(this.messageWindow).First().RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }));
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
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

        [Then(@"the message box also closes")]
        public void ThenTheMessageBoxAlsoCloses()
        {
            Assert.IsFalse(this.messageWindow.IsVisible, "Message Window is visible.");
            this.messageWindow.Dispatcher.InvokeShutdown();
            int escapeCount = 0;

            while (this.messageWindow.Dispatcher.HasShutdownFinished && escapeCount < 10)
            {
                Thread.Sleep(500);
                escapeCount++;
            }
        }

        [Given(@"I am using '(.*)' automation framework")]
        public void GivenIAmUsingAutomationFramework(string framework)
        {
            switch (framework)
            {
                case "white":
                    this.messageWindowPO = new SimpleMessageBoxPageObject_White();
                    break;
                case "flaui":
                    this.messageWindowPO = new SimpleMessageBoxPageObject_FlaUI();
                    break;
                case "appium":
                    this.messageWindowPO = new SimpleMessageBoxPageObject_Appium();
                    break;
                default:
                    ScenarioContext.Current.Pending();
                    break;
            }

            Assert.IsTrue(this.messageWindowPO.WaitForMainWindowToLoad(30000), "Main window didn't load within timeout.");
        }


        private void ShowMessageAsync()
        {
            this.messageThread = new Thread(new ThreadStart(() =>
            {
                MessageBoxTestHelper helper = new MessageBoxTestHelper();
                helper.ShowMessageBox();
                Dispatcher.Run();
            }));

            this.messageThread.SetApartmentState(ApartmentState.STA);
            this.messageThread.Start();
        }

        [When(@"I use UI Automation to click ok")]
        public void WhenIUseUiAutomationToClickOk()
        {
            this.messageWindowPO.ClickOkButton();

            int escapeCount = 0;
            while (this.messageWindowPO.Exists && escapeCount < 10)
            {
                Thread.Sleep(500);
            }
        }

        [Then(@"the message box is no longer on screen")]
        public void ThenTheMessageBoxIsNoLongerOnScreen()
        {
            Assert.IsTrue(this.messageWindowPO.MainWindowIsClosed);

            // clean-up window page object
            this.messageWindowPO.DisposeMainWindow();
        }
    }
}
