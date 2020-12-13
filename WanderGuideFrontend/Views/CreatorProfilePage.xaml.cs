using System;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;

namespace WanderGuideFrontend.Views
{
    public partial class CreatorProfilePage : ContentPage
    {
        private readonly CreatorProfileViewModel viewModel;

        public CreatorProfilePage(string user_id)
        {
            InitializeComponent();
            BindingContext = viewModel = new CreatorProfileViewModel(user_id);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadProfileInformation();
        }
    }
}