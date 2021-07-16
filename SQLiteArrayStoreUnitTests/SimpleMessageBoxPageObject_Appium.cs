using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SQLiteArrayStoreUnitTests
{
    public class SimpleMessageBoxPageObject_Appium : ISimpleMessageBoxAutomation
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        private static WindowsDriver<WindowsElement> messageBoxSession;
        private static WindowsDriver<WindowsElement> desktopSession;

        private WindowsElement mainWindow;

        internal WindowsElement MainWindow
        {
            get
            {
                if (mainWindow == null)
                {
                    InitializeTestSession();
                }

                return mainWindow;
            }
        }

        private WindowsElement okButton;

        internal WindowsElement OkButton
        {
            get
            {
                if (okButton == null)
                {
                    okButton = messageBoxSession.FindElementByAccessibilityId(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxOkButtonID);
                }
                return okButton;
            }
        }

        private void InitializeTestSession()
        {
            StartWinAppDriver();

            Thread.Sleep(3000); // speeds up appium by not 'missing' the first attempt to find the window and then going through the timeouts.

            // Create a session for Desktop
            AppiumOptions desktopOptions = new AppiumOptions();
            desktopOptions.AddAdditionalCapability("app", "Root");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), desktopOptions);
            Assert.IsNotNull(desktopSession);

            //desktopSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

            mainWindow = desktopSession.FindElementByAccessibilityId(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID);

            string mainWindowTopLevelWindowHandle = (int.Parse(mainWindow.GetAttribute("NativeWindowHandle"))).ToString("x");

            // Create session for messageBox window
            AppiumOptions messageboxOptions = new AppiumOptions();
            messageboxOptions.AddAdditionalCapability("appTopLevelWindow", mainWindowTopLevelWindowHandle);
            messageBoxSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), messageboxOptions);
            Assert.IsNotNull(messageBoxSession);

            //messageBoxSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0.5));
        }

        private static void StartWinAppDriver()
        {
            string wadExeName = "WinAppDriver";

            if (!Process.GetProcessesByName(wadExeName).Any())
            {
                string wadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Windows Application Driver");

                Assert.IsTrue(Directory.Exists(wadFolder), "Windows Application Driver must be installed for this test to work.");

                ProcessStartInfo wadInfo = new ProcessStartInfo($"{wadFolder}\\{wadExeName}.exe");
                wadInfo.WorkingDirectory = wadFolder;
                wadInfo.UseShellExecute = true;

                Process wadProcess = Process.Start(wadInfo);
                Thread.Sleep(1000);

                Assert.IsFalse(wadProcess.HasExited, "WAD process exited. Windows must be in Developer Mode to run 'Windows Application Driver'.");
            }
        }

        public bool Exists
        {
            get
            {
                WindowsElement window;
                //desktopSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0.5));

                try
                {
                    window = desktopSession.FindElementByAccessibilityId(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID);
                }
                catch (OpenQA.Selenium.WebDriverException wde)
                {
                    window = null;
                    Console.WriteLine("Exception thrown: " + wde.Message);
                }
                catch (InvalidOperationException ioe)
                {
                    window = null;
                    Console.WriteLine("Exception thrown: " + ioe.Message);
                }

                return window != null;
            }
        }

        public bool MainWindowIsClosed
        {
            get
            {
                return !Exists;
            }
        }

        public void ClickOkButton()
        {
            OkButton.Click();
        }

        public void DisposeMainWindow()
        {
            messageBoxSession.Dispose();
            desktopSession.Dispose();
        }

        public bool WaitForMainWindowToLoad(int timeoutInMilliSeconds)
        {
            int escapeCount = 0;
            int sleeptime = timeoutInMilliSeconds / 30;

            while (MainWindow == null && escapeCount < 30)
            {
                Thread.Sleep(sleeptime);
                escapeCount++;
            }

            return escapeCount < 30;
        }
    }
}
