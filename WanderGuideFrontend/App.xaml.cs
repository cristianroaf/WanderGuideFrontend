using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WanderGuideFrontend.Services;
using WanderGuideFrontend.Views;

namespace WanderGuideFrontend
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
