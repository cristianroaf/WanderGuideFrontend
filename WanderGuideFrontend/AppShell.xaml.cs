using System;
using System.Collections.Generic;
using WanderGuideFrontend.ViewModels;
using WanderGuideFrontend.Views;
using Xamarin.Forms;

namespace WanderGuideFrontend
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

    }
}
