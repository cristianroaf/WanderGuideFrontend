using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuideMapPage : ContentPage
    {
        private readonly GuideMapViewModel viewModel;
        public GuideMapPage(string guide_id)
        {
            InitializeComponent();
            BindingContext = viewModel = new GuideMapViewModel(guide_id);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}