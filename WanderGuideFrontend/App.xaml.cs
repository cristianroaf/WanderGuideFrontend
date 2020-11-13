using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WanderGuideFrontend.Services;
using WanderGuideFrontend.Views;

namespace WanderGuideFrontend
{
    public partial class App : Application
    {
        public static string User_id { get; set; }
        public static string Username { get; set; }
        public App()
        {
            InitializeComponent();

            User_id = "";
            Username = "";

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Shell.Current.GoToAsync("//LoginPage");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
