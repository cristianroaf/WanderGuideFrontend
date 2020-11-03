using System.ComponentModel;
using Xamarin.Forms;
using WanderGuideFrontend.ViewModels;

namespace WanderGuideFrontend.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}