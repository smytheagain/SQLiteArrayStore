namespace SQLiteArrayStoreUnitTests
{
    public class MessageBoxTestHelper
    {
        public MessageBoxTestHelper()
        {
            MessageBoxInstance = new SimpleMessageBox();
        }

        public SimpleMessageBox MessageBoxInstance { get; set; }

        public void ShowMessageBox()
        {
            MessageBoxInstance.Show();
        }
    }
}
