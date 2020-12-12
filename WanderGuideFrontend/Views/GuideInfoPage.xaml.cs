using System;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuideInfoPage : ContentPage
    {
        private readonly GuideInfoViewModel viewModel;
        public GuideInfoPage(string id)
        {
            InitializeComponent();
            BindingContext = viewModel = new GuideInfoViewModel(id);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }

        private void WatchOnTheMap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GuideMapPage(viewModel.guide_id));
        }

        private void GoToCreator(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new CreatorProfilePage(viewModel.guide.user_id));
        }
    }
}
