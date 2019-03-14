using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;

namespace SQLiteArrayStoreUnitTests
{
    [Binding]
    public class CloseDialogTestSteps
    {
        private Thread messageThread;
        private TestDataHelper helperInstance;

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
                this.helperInstance = new TestDataHelper();
                this.helperInstance.ShowMessageBox();

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
                this.helperInstance.MessageBoxInstance.Close();
            }));
        }
        
        [Then(@"the message box closes")]
        public void ThenTheMessageBoxCloses()
        {
            bool messageBoxVisible = Dispatcher.FromThread(messageThread).Invoke(new Func<bool>(() =>
            {
                return this.helperInstance.MessageBoxInstance.IsVisible;
            }), DispatcherPriority.Background);

            messageThread.Abort();

            Assert.AreEqual(false, messageBoxVisible);
        }

        [When(@"I invoke the ok button click event")]
        public void WhenIInvokeTheOkButtonClickEvent()
        {
            Thread.Sleep(2000);

            Dispatcher.FromThread(messageThread).Invoke(DispatcherPriority.Background, new Action(() =>
            {
                this.helperInstance.MessageBoxInstance.okButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }));
        }

    }
}
