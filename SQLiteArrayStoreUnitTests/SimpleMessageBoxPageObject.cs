using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Linq;
using System;
using System.Windows.Automation;

namespace SQLiteArrayStoreUnitTests
{
    public class SimpleMessageBoxPageObject
    {
        private Button okButton;
        private Window mainWindow;

        public Window MainWindow
        {
            get
            {
                if (this.mainWindow == null)
                {
                    this.mainWindow = Desktop.Instance.Windows().FirstOrDefault(w => w.Id == "MessageBoxWindow");
                }

                return this.mainWindow;
            }
        }

        public Button OkButton
        {
            get
            {
                if (this.okButton == null)
                {
                    this.okButton = this.mainWindow.Get<Button>(SearchCriteria.ByAutomationId("okButton"));
                }

                return this.okButton;
            }
        }

        public bool Exists
        {
            get
            {
                bool doesExist = false;
                try
                {
                    doesExist = Desktop.Instance.Windows().Contains(this.mainWindow);
                }
                catch (ElementNotAvailableException e)
                {
                    Console.WriteLine(e.GetType() + ", " + e.Message);
                }

                //AutomationElement desktop = AutomationElement.RootElement;
                //AutomationElement messageWindowElement = desktop.FindFirst(
                //    TreeScope.Children, 
                //    new PropertyCondition(AutomationElement.AutomationIdProperty, "MessageBoxWindow"));

                return doesExist;
            }
        }

    }
}
