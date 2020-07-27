using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Demo.Views;

namespace MintPlayer.MVVM.Demo
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
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
