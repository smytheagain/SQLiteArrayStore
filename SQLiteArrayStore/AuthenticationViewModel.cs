using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SQLiteArrayStore
{
    /// <summary>
    /// Represents the View Model for the Authentication View.
    /// </summary>
    internal class AuthenticationViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private string _userNameLabel;
        private string _passwordLabel;
        private string _okButtonText;
        private string _cancelButtonText;
        private string _description;

        /// <summary>
        /// Raised when the authentication has finished.
        /// </summary>
        internal event EventHandler<EventArgs> Finished;

        /// <summary>
        /// Raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the OK relay command.
        /// </summary>
        public ICommand CommandOk { get; }

        /// <summary>
        /// Gets the Cancel relay command.
        /// </summary>
        public ICommand CommandCancel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationViewModel"/> class.
        /// </summary>
        internal AuthenticationViewModel()
        {
            _userName = string.Empty;
            _userNameLabel = "Username:";
            _passwordLabel = "Password:";
            _okButtonText = "OK";
            _cancelButtonText = "Cancel";

            CommandOk = new RelayCommand(p => Ok());
            CommandCancel = new RelayCommand(p => Cancel());
        }

        /// <summary>
        /// Gets the result after authentication has completed.
        /// </summary>
        internal AuthenticationResult LoginResult { get; private set; }

        /// <summary>
        /// Gets or sets the user name entered by the user.
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                if (!_userName.Equals(value, StringComparison.Ordinal))
                {
                    _userName = value;
                    NotifyOfPropertyChanged(nameof(UserName));
                }
            }
        }

        /// <summary>
        /// Gets the user name label.
        /// </summary>
        public string UserNameLabel
        {
            get => _userNameLabel;

            private set
            {
                if (!_userNameLabel.Equals(value, StringComparison.Ordinal))
                {
                    _userNameLabel = value;
                    NotifyOfPropertyChanged(nameof(UserNameLabel));
                }
            }
        }

        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        internal string Password { get; set; }

        /// <summary>
        /// Gets the password label.
        /// </summary>
        public string PasswordLabel
        {
            get => _passwordLabel;

            private set
            {
                if (!_passwordLabel.Equals(value, StringComparison.Ordinal))
                {
                    _passwordLabel = value;
                    NotifyOfPropertyChanged(nameof(PasswordLabel));
                }
            }
        }

        /// <summary>
        /// Cancels the authentication operation.
        /// </summary>
        internal void Cancel()
        {
            LoginResult = AuthenticationResult.Cancelled;
            Finished?.Invoke(this, null);
        }

        /// <summary>
        /// Finishes the authentication operation by entering the user credentials.
        /// </summary>
        internal void Ok()
        {
            if (AreCredentialsCorrect(UserName, Password))
            {
                LoginResult = AuthenticationResult.Authenticated;
            }
            else
            {
                LoginResult = AuthenticationResult.Failed;
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public bool AreCredentialsCorrect(string userName, string password)
        {
            return (userName == "testUser" && password == "T3stP@55w0rd");
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get => _description;

            private set
            {
                if (!_description.Equals(value, StringComparison.Ordinal))
                {
                    _description = value;
                    NotifyOfPropertyChanged(nameof(Description));
                }
            }
        }

        /// <summary>
        /// Gets the OK button text.
        /// </summary>
        public string OkButtonText
        {
            get => _okButtonText;

            private set
            {
                if (!_okButtonText.Equals(value, StringComparison.Ordinal))
                {
                    _okButtonText = value;
                    NotifyOfPropertyChanged(nameof(OkButtonText));
                }
            }
        }

        /// <summary>
        /// Gets the cancel button text.
        /// </summary>
        public string CancelButtonText
        {
            get => _cancelButtonText;

            private set
            {
                if (!_cancelButtonText.Equals(value, StringComparison.Ordinal))
                {
                    _cancelButtonText = value;
                    NotifyOfPropertyChanged(nameof(CancelButtonText));
                }
            }
        }

        /// <summary>
        /// Raises a PropertyChanged event.
        /// </summary>
        /// <param name="prop">The name of the property that changed.</param>
        internal void NotifyOfPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }   
}
