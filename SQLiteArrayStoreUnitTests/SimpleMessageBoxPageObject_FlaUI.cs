using FlaUI.Core.AutomationElements;
using FlaUI.Core.AutomationElements.Infrastructure;
using FlaUI.UIA3;
using System.Threading;

namespace SQLiteArrayStoreUnitTests
{
    public class SimpleMessageBoxPageObject_FlaUI : ISimpleMessageBoxAutomation
    {
        private Window mainWindow;
        private Button okButton;

        internal Window MainWindow
        {
            get
            {
                if (this.mainWindow == null)
                {
                    Thread.Sleep(5000); // need to implement a search timeout!
                    AutomationElement desktop = new UIA3Automation().GetDesktop();
                    this.mainWindow = desktop.FindFirstChild(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID).AsWindow();
                }

                return this.mainWindow;
            }
        }

        internal Button OkButton
        {
            get
            {
                if (this.okButton == null)
                {
                    this.okButton = MainWindow.FindFirstChild(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxOkButtonID).AsButton();
                }

                return this.okButton;
            }
        }

        public bool Exists
        {
            get
            {
                AutomationElement messageWindowElement = new UIA3Automation().GetDesktop().FindFirstChild(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID);

                bool doesExist = messageWindowElement != null;

                return doesExist;
            }
        }

        public bool MainWindowIsClosed
        {
            get
            {
                return MainWindow.IsOffscreen;
            }
        }


        public void ClickOkButton()
        {
            OkButton.Click();
        }

        public void DisposeMainWindow()
        {
            MainWindow.Automation.Dispose();
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
