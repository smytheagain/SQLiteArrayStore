using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Linq;
using System;
using System.Windows.Automation;
using System.Threading;

namespace SQLiteArrayStoreUnitTests
{
    public class SimpleMessageBoxPageObject_White : ISimpleMessageBoxAutomation
    {
        private Button okButton;
        private Window mainWindow;

        internal Window MainWindow
        {
            get
            {
                if (this.mainWindow == null)
                {
                    this.mainWindow = Desktop.Instance.Windows().FirstOrDefault(w => w.Id == SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID);
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
                    this.okButton = this.mainWindow.Get<Button>(SearchCriteria.ByAutomationId(SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxOkButtonID));
                }

                return this.okButton;
            }
        }

        public bool Exists
        {
            get
            {
                AutomationElement desktop = AutomationElement.RootElement;
                AutomationElement messageWindowElement = desktop.FindFirst(
                    TreeScope.Children,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, SQLiteArrayStore.Resources.AutomationIds.SimpleMessageBoxWindowID));

                bool doesExist = messageWindowElement != null;

                return doesExist;
            }
        }

        public bool MainWindowIsClosed
        {
            get
            {
                return this.MainWindow.IsClosed;
            }
        }

        public void ClickOkButton()
        {
            this.OkButton.Click();
        }

        public void DisposeMainWindow()
        {
            this.MainWindow.Dispose();
        }

        public bool WaitForMainWindowToLoad(int timeoutInMilliSeconds)
        {
            int escapeCount = 0;
            int sleeptime = timeoutInMilliSeconds / 30;
            
            while (this.MainWindow == null && escapeCount < 30)
            {
                Thread.Sleep(sleeptime);
                escapeCount++;
            }

            this.MainWindow.WaitWhileBusy();

            return escapeCount < 30;
        }
    }
}
