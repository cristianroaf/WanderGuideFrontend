using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;

namespace WanderGuideFrontend.Views
{
    public partial class CreateStopPage : ContentPage
    {
        private readonly CreateStopViewModel viewModel;
        public CreateStopPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CreateStopViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}