using Plugin.Toast;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using WanderGuideFrontend.Models;
using WanderGuideFrontend.Views;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    public class GuideInfoViewModel : BaseViewModel
    {
        #region Attribute
        public List<Stop> stops;
        public Guide guide;
        public string guide_id;
        public string username;
        public string rating;
        public string name;
        public string description;
        public string age;
        public string title;
        public string button;
        public bool guideOwner;

        public bool isEnabled = false;
        #endregion
        #region Properties
        public bool isGuideOwner
        {
            get => guideOwner;
            set => SetValue(ref guideOwner, value);
        }
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

        public string RatingTxt
        {
            get => rating;
            set => SetValue(ref rating, value);
        }

        public bool IsEnabledTxt
        {
            get => isEnabled;
            set => SetValue(ref isEnabled, value);
        }
        #endregion
        public Command RateCommand { get; }
        public Command DeleteCommand { get; }

        public GuideInfoViewModel(string id)
        {
            guide_id = id;
            RateCommand = new Command(RateMethod);
            DeleteCommand = new Command(DeleteMethod);
            guideOwner = false;
        }
        public async void DeleteMethod()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Warning", "Are you sure you want to delete the guide? All the interest points and ratings will be lost", "Yes", "No");
            if (answer)
            {
                Services.RestService connection = new Services.RestService();
                int statuscode = await connection.DeleteGuide(App.User_id, guide._id);
                if (statuscode == 200)
                {
                    CrossToastPopUp.Current.ShowToastMessage("Deleted guide successfully");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }
        public async void RateMethod()
        {

            RatePopup popupProperties = new RatePopup(guide_id, this);
            ScaleAnimation scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Left
            };

            popupProperties.Animation = scaleAnimation;

            await PopupNavigation.Instance.PushAsync(popupProperties, true);
        }

        internal async void OnAppearing()
        {
            StopsList = new List<Stop>();
            TitleTxt = "";
            Services.RestService connection = new Services.RestService();
            (Guide g, int statuscode1) = await connection.GetPublishedGuide(guide_id);
            (List<Stop> s, int statuscode2) = await connection.GetPublishedStops(guide_id);
            if (statuscode1 == 200)
            {
                guide = g;
                TitleTxt = g.title;
                UsernameTxt = "By: " + g.creator_username;
                RatingTxt = Math.Round(g.rating, 1).ToString() + "/10";
                isGuideOwner = (App.User_id == g.user_id);
            }
            if (statuscode2 == 200)
            {
                StopsList = s;
            }
        }
    }
}