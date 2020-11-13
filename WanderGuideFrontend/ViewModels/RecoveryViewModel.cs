using Plugin.Toast;
using WanderGuideFrontend.Views;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    internal class RecoveryViewModel : BaseViewModel
    {

        #region Attributes
        public string email;

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
        public Command RecoveryCommand { get; }
        #endregion


        public RecoveryViewModel()
        {
            RecoveryCommand = new Command(RecoveryMethod);
        }

        private async void RecoveryMethod()
        {

            if (string.IsNullOrEmpty(email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email",
                    "Accept");
                return;
            }

            IsVisibleTxt = true;
            IsRunningTxt = true;
            IsEnabledTxt = false;

            Services.RestService connection = new Services.RestService();
            int statuscode = await connection.PasswordRecovery(EmailTxt);
            IsVisibleTxt = false;
            IsRunningTxt = false;
            IsEnabledTxt = true;
            if (statuscode == 411)
            {
                CrossToastPopUp.Current.ShowToastMessage("There is no user with such email");
            }
            else if (statuscode == 408)
            {
                CrossToastPopUp.Current.ShowToastMessage("Connection timed out");
            }
            else if (statuscode == 400)
            {
                CrossToastPopUp.Current.ShowToastMessage("Something went wrong");
            }
            else if (statuscode == 200)
            {
                CrossToastPopUp.Current.ShowToastMessage("Recovery email sent");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
