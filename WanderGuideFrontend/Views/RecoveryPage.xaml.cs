using System;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoveryPage : ContentPage
    {
        public RecoveryPage()
        {
            InitializeComponent();
            BindingContext = new RecoveryViewModel();
        }

        private async void GoToLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}