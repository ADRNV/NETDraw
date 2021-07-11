using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NETDraw.Pages;

namespace NETDraw
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new WorkPage();
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
