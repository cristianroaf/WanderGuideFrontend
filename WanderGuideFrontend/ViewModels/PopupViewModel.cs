using Plugin.Toast;
using Rg.Plugins.Popup.Services;
using WanderGuideFrontend.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace WanderGuideFrontend.ViewModels
{
    public class PopupViewViewModel : BaseViewModel
    {
        #region Attribute
        public string title;
        public string description;
        private readonly double latitude;
        private readonly double longitude;
        public CreateStopViewModel prevmodel;
        #endregion

        #region Properties
        public string TitleTxt
        {
            get => title;
            set => SetValue(ref title, value);
        }
        public string DescriptionTxt
        {
            get => description;
            set => SetValue(ref description, value);
        }
        #endregion

        #region Commands
        public Command BackCommand { get; }
        public Command SavedPointCommand { get; }
        #endregion

        public PopupViewViewModel(CreateStopViewModel cmodel, Position p)
        {
            BackCommand = new Command(BackMethod);
            SavedPointCommand = new Command(SavedPointMethod);
            latitude = p.Latitude;
            longitude = p.Longitude;
            prevmodel = cmodel;
        }
        private async void SavedPointMethod()
        {
            if (string.IsNullOrEmpty(title))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must provide a title",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Attention", "Are you sure you want to save without description?", "Yes", "No");
                if (!answer)
                {
                    return;
                }
                description = " ";
            }
            RestService connection = new RestService();
            int statuscode = await connection.CreateStop(App.User_id, title, description, latitude, longitude);
            if (statuscode == 200)
            {
                prevmodel.ReloadPins();
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                CrossToastPopUp.Current.ShowToastMessage("An error ocurred saving to the database");
                BackMethod();
            }
        }
        private async void BackMethod()
        {
            title = "";
            description = "";
            prevmodel.PopUpBackPressed();
            await PopupNavigation.Instance.PopAsync();
        }

    }
}
