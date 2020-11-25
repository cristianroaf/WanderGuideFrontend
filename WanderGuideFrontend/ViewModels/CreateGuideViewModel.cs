using Plugin.Toast;
using System.Collections.Generic;
using WanderGuideFrontend.Models;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    internal class CreateGuideViewModel : BaseViewModel
    {
        #region Attribute
        public List<Stop> stops;
        public Guide guide;
        public ImageSource profile_image;
        public Entry titleEntry;
        public string username;
        public string name;
        public string description;
        public string age;
        public string title;
        public string button;

        public bool isEnabled = false;
        #endregion
        #region Properties
        public string TitleTxt
        {
            get => title;
            set => SetValue(ref title, value);
        }
        public List<Stop> StopsList
        {
            get => stops;
            set => SetValue(ref stops, value);
        }

        public string DescriptionTxt
        {
            get => description;
            set => SetValue(ref description, value);
        }
        public string AgeTxt
        {
            get => age;
            set => SetValue(ref age, value);
        }
        public string ButtonTxt
        {
            get => button;
            set => SetValue(ref button, value);
        }
        public string NameTxt
        {
            get => name;
            set => SetValue(ref name, value);
        }
        public string UsernameTxt
        {
            get => username;
            set => SetValue(ref username, value);
        }

        public ImageSource ProfilePic
        {
            get => profile_image;
            set => SetValue(ref profile_image, value);
        }
        public bool IsEnabledTxt
        {
            get => isEnabled;
            set => SetValue(ref isEnabled, value);
        }
        #endregion
        public Command PublishCommand { get; }
        public Command DeleteDraftCommand { get; }
        public CreateGuideViewModel()
        {
            PublishCommand = new Command(PublishMethod);
            DeleteDraftCommand = new Command(DeleteDraftMethod);
            StopsList = new List<Stop>();
            LoadDraftGuide();
        }
        public async void UpdateTitleMethod()
        {
            Services.RestService connection = new Services.RestService();
            int statuscode = await connection.UpdateDraftTitle(App.User_id, TitleTxt);
            if (statuscode != 200)
            {
                CrossToastPopUp.Current.ShowToastMessage("An error ocurred saving the title");
            }
        }
        public async void PublishMethod()
        {

            if (string.IsNullOrEmpty(title))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Make sure to add a title to your guide",
                    "Accept");
                return;
            }
            if (stops.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "It seems like your guide has no interest points",
                    "Accept");
                return;
            }
            bool answer = await Application.Current.MainPage.DisplayAlert("Attention", "Are you sure you want to publish the guide?", "Yes", "No");
            if (answer)
            {
                Services.RestService connection = new Services.RestService();
                int statuscode = await connection.PublishDraftGuide(App.User_id);
                if (statuscode == 200)
                {
                    CrossToastPopUp.Current.ShowToastMessage("Guide successfully published");
                }
            }

            LoadDraftGuide();
        }
        public async void DeletePointMethod(string id)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Attention", "Are you sure you want to delete the interest point?", "Yes", "No");
            if (answer)
            {
                Services.RestService connection = new Services.RestService();
                int statuscode = await connection.DeleteStop(id);
                if (statuscode == 200)
                {
                    CrossToastPopUp.Current.ShowToastMessage("Deleted interest point successfully");
                }

                LoadDraftGuide();
            }

        }
        public async void DeleteDraftMethod()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Attention", "Are you sure you want to delete the draft?", "Yes", "No");
            if (answer)
            {
                Services.RestService connection = new Services.RestService();
                int statuscode = await connection.DeleteGuide(App.User_id, guide._id);
                if (statuscode == 200)
                {
                    CrossToastPopUp.Current.ShowToastMessage("Deleted draft guide successfully");
                }

                LoadDraftGuide();
            }
        }
        public async void LoadDraftGuide()
        {

            Services.RestService connection = new Services.RestService();
            (Guide g, int statuscode1) = await connection.GetDraftGuide(App.User_id);
            (List<Stop> s, int statuscode2) = await connection.GetDraftStops(App.User_id);
            StopsList = new List<Stop>();
            TitleTxt = "";
            if (statuscode1 == 200)
            {
                guide = g;
                TitleTxt = g.title;
                titleEntry.Unfocus();
            }
            if (statuscode2 == 200)
            {
                StopsList = s;
            }
        }
    }
}

