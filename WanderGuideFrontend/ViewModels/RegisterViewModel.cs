using Plugin.Toast;
using WanderGuideFrontend.Views;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Attributes
        public string email;
        public string password;
        public string username;

        public bool isRunning = false;
        public bool isVisible = false;
        public bool isEnabled = true;
        #endregion

        #region Properties
        public string EmailTxt
        {
            get => email;
            set => SetValue(ref email, value);
        }
        public string PasswordTxt
        {
            get => password;
            set => SetValue(ref password, value);
        }
        public string UsernameTxt
        {
            get => username;
            set => SetValue(ref username, value);
        }
        public bool IsVisibleTxt
        {
            get => isVisible;
            set => SetValue(ref isVisible, value);
        }
        public bool IsEnabledTxt
        {
            get => isEnabled;
            set => SetValue(ref isEnabled, value);
        }
        public bool IsRunningTxt
        {
            get => isRunning;
            set => SetValue(ref isRunning, value);
        }
        #endregion

        #region Commands
        public Command RegisterCommand { get; }
        #endregion

        public RegisterViewModel()
        {
            RegisterCommand = new Command(RegisterMethod);
        }

        private async void RegisterMethod()
        {
            if (string.IsNullOrEmpty(email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(username))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an username",
                    "Accept");
                return;
            }

            IsVisibleTxt = true;
            IsRunningTxt = true;
            IsEnabledTxt = false;

            Services.RestService connection = new Services.RestService();
            int statuscode = await connection.Register(email, username, password);

            IsVisibleTxt = false;
            IsRunningTxt = false;
            IsEnabledTxt = true;

            if (statuscode == 410)
            {
                CrossToastPopUp.Current.ShowToastMessage("Provided email is not valid");
            }
            else if (statuscode == 411)
            {
                CrossToastPopUp.Current.ShowToastMessage("Username or email already in use");
            }
            else if (statuscode == 408)
            {
                CrossToastPopUp.Current.ShowToastMessage("Connection timed out");
            }
            else if (statuscode == 200)
            {
                CrossToastPopUp.Current.ShowToastMessage("Verification email sent");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
