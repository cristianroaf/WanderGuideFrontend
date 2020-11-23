using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WanderGuideFrontend.ViewModels;

namespace WanderGuideFrontend.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly MapViewModel viewModel;
        public MapPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MapViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}