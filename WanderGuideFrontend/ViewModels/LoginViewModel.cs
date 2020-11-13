using Plugin.Toast;
using System;
using WanderGuideFrontend.Models;
using WanderGuideFrontend.Services;
using WanderGuideFrontend.Views;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attribute
        public string username;
        public string password;

        public bool isRunning = false;
        public bool isVisible = false;
        public bool isEnabled = true;
        public bool isToggled;
        public bool IsToggledAutologin;
        #endregion

        #region Properties
        public string UsernameTxt
        {
            get => username;
            set => SetValue(ref username, value);
        }
        public string PasswordTxt
        {
            get => password;
            set => SetValue(ref password, value);
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
        public bool IsToggledTxt
        {
            get => isToggled;
            set => SetValue(ref isToggled, value);
        }
        public bool IsToggledAutologinTxt
        {
            get => IsToggledAutologin;
            set => SetValue(ref IsToggledAutologin, value);
        }
        #endregion

        #region Commands
        public Command LoginCommand { get; }
        #endregion

        public LoginViewModel()
        {

            LoginCommand = new Command(LoginMethod);
            IsToggledTxt = Settings.RememberMe;
            if (IsToggledTxt)
            {
                UsernameTxt = Settings.LastUsedUsername;
                PasswordTxt = Settings.LastUsedPassword;
            }
            IsToggledAutologinTxt = Settings.Autologin;
            if (IsToggledAutologinTxt)
            {
                LoginMethod();
            }
        }
        public void OnAppearing()
        {
            IsToggledTxt = Settings.RememberMe;
            if (IsToggledTxt)
            {
                UsernameTxt = Settings.LastUsedUsername;
                PasswordTxt = Settings.LastUsedPassword;
            }
            IsToggledAutologinTxt = Settings.Autologin;
            Console.WriteLine(IsToggledTxt);
            Console.WriteLine(IsToggledAutologinTxt);
            if (IsToggledAutologinTxt)
            {
                LoginMethod();
            }
        }
        private async void LoginMethod()
        {
            if (string.IsNullOrEmpty(username))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an username",
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

            IsVisibleTxt = true;
            IsRunningTxt = true;
            IsEnabledTxt = false;

            RestService connection = new Services.RestService();
            (User u, int statuscode) = await connection.Login(username, password);


            if (statuscode == 200)
            {
                App.User_id = u._id;
                App.Username = u.Username;
                CrossToastPopUp.Current.ShowToastMessage("Log In successful");
                if (IsToggledTxt)
                {
                    Settings.LastUsedUsername = UsernameTxt;
                    Settings.LastUsedPassword = PasswordTxt;
                    Settings.RememberMe = IsToggledTxt;
                    Settings.Autologin = IsToggledAutologinTxt;
                }
                else
                {
                    UsernameTxt = "";
                    PasswordTxt = "";
                    Settings.LastUsedUsername = "";
                    Settings.LastUsedPassword = "";
                    Settings.RememberMe = IsToggledTxt;
                }
                await Shell.Current.GoToAsync($"//MapPage");
            }
            else if (statuscode == 408)
            {
                CrossToastPopUp.Current.ShowToastMessage("Connection timed out");
            }
            else if (statuscode == 411)
            {
                CrossToastPopUp.Current.ShowToastMessage("Username does not exist");
            }
            else if (statuscode == 412)
            {
                CrossToastPopUp.Current.ShowToastMessage("Please verify your email address");
            }
            else if (statuscode == 410)
            {
                CrossToastPopUp.Current.ShowToastMessage("Incorrect password");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastMessage("Unknown error");
            }

            IsVisibleTxt = false;
            IsRunningTxt = false;
            IsEnabledTxt = true;
        }
    }
}
