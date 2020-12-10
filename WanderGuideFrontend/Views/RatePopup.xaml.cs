using Rg.Plugins.Popup.Pages;
using System;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatePopup : PopupPage
    {
        private readonly RatePopupViewModel viewModel;
        public RatePopup(string guideId, GuideInfoViewModel prevmodel)
        {
            InitializeComponent();
            BindingContext = viewModel = new RatePopupViewModel(guideId, prevmodel);
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            viewModel.SliderChange(Math.Floor(((Slider)sender).Value).ToString());
        }
    }
}