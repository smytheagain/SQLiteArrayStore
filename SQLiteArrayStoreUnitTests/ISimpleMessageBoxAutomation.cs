namespace SQLiteArrayStoreUnitTests
{
    public interface ISimpleMessageBoxAutomation
    {
        bool Exists { get; }
        bool WaitForMainWindowToLoad(int timeoutInMilliSeconds);
        bool MainWindowIsClosed { get; }
        void DisposeMainWindow();
        void ClickOkButton();
    }
}