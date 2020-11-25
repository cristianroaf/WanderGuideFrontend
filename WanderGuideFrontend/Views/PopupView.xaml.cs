using Rg.Plugins.Popup.Pages;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupView : PopupPage
    {
        public PopupView(CreateStopViewModel model, Position pos)
        {
            InitializeComponent();
            BindingContext = new PopupViewViewModel(model, pos);
        }
    }
}