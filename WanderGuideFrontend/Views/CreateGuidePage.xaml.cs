using System;
using System.Collections.Generic;
using WanderGuideFrontend.Models;
using WanderGuideFrontend.ViewModels;
using Xamarin.Forms;

namespace WanderGuideFrontend.Views
{
    public partial class CreateGuidePage : ContentPage
    {
        private readonly CreateGuideViewModel viewModel;
        public IList<Stop> StopsList { get; private set; }

        public CreateGuidePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CreateGuideViewModel();
            viewModel.titleEntry = title_entry;
        }
        private async void GoToCreateStop_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateStopPage());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadDraftGuide();
        }
        public void DeletePoint(object Sender, EventArgs args)
        {
            ImageButton button = (ImageButton)Sender;
            string ID = button.CommandParameter.ToString();
            viewModel.DeletePointMethod(ID);
            viewModel.LoadDraftGuide();
        }

        private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Stop selectedItem = e.SelectedItem as Stop;
        }

        private void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Stop tappedItem = e.Item as Stop;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.UpdateTitleMethod();
        }
    }
}