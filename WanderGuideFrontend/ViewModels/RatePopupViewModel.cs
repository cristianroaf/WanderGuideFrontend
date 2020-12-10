using Plugin.Toast;
using Rg.Plugins.Popup.Services;
using System;
using WanderGuideFrontend.Services;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    public class RatePopupViewModel : BaseViewModel
    {
        #region Attribute
        private readonly string guide_id;
        public string value;
        public double slider;
        public GuideInfoViewModel prevmodel;
        #endregion

        #region Properties
        public double SliderValue
        {
            get => slider;
            set => SetValue(ref slider, value);
        }
        internal void SliderChange(string v)
        {
            ValueTxt = v;
        }

        public string ValueTxt
        {
            get => value;
            set => SetValue(ref this.value, value);
        }
        #endregion

        #region Commands
        public Command BackCommand { get; }
        public Command SaveRatingCommand { get; }
        #endregion

        public RatePopupViewModel(string guideId, GuideInfoViewModel prevmodel)
        {
            BackCommand = new Command(BackMethod);
            SaveRatingCommand = new Command(SaveRatingMethod);
            SliderValue = 5;
            guide_id = guideId;
            this.prevmodel = prevmodel;
        }
        private async void SaveRatingMethod()
        {
            RestService connection = new RestService();
            int statuscode = await connection.RateGuide(App.User_id, guide_id, Math.Floor(slider));

            if (statuscode == 200)
            {
                CrossToastPopUp.Current.ShowToastMessage("Successfully rated the guide");
            }
            else if (statuscode == 201)
            {
                CrossToastPopUp.Current.ShowToastMessage("The guide no longer exists");
            }
            else if (statuscode == 202)
            {
                CrossToastPopUp.Current.ShowToastMessage("You can't rate your own guide");
            }
            else if (statuscode == 203)
            {
                CrossToastPopUp.Current.ShowToastMessage("Already rated this guide");
            }

            prevmodel.OnAppearing();
            BackMethod();

        }
        private async void BackMethod()
        {
            await PopupNavigation.Instance.PopAsync();
        }

    }
}
