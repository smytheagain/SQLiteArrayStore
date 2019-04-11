using System;
using System.Windows;

namespace SQLiteArrayStore
{
    internal class Authenticate : IAuthenticate
    {
        public Authenticate()
        {
        }

        public AuthenticationResult AuthenticateUser()
        {
            AuthenticationViewModel authenticationViewModel = new AuthenticationViewModel();

            ShowDialog(authenticationViewModel);

            return authenticationViewModel.LoginResult;
        }

        internal void ShowDialog(AuthenticationViewModel authenticationViewModel)
        {
            Window authenticationView = new AuthenticationView()
            {
                DataContext = authenticationViewModel,
            };

            authenticationViewModel.Finished += OnAuthenticationViewModelFinished(authenticationView);

            authenticationView.ShowDialog();

            authenticationViewModel.Finished -= OnAuthenticationViewModelFinished(authenticationView);
        }

        private static EventHandler<EventArgs> OnAuthenticationViewModelFinished(Window authenticationView)
        {
            return (s, e) => authenticationView.Close();
        }
    }
}
