using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WanderGuideFrontend.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProfileViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadProfileInformation();
        }

        public async void OnProfileImageClicked(object sender, EventArgs e)
        {
            if (viewModel.IsEnabledPhotoTxt)
            {
                await viewModel.ProfileImageClicked();
            }
        }
    }
}