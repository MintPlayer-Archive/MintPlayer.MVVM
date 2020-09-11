using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Demo.Views;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Demo.ViewModels;

namespace MintPlayer.MVVM.Demo
{
    public partial class App : Application
    {
        private readonly INavigationService navigationService;
        public App(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        
            InitializeComponent();
            MainPage = new MainPage();

            navigationService.SetMainNavigation(((MasterDetailPage)MainPage).Detail.Navigation);
            navigationService.SetMainPage<ItemsVM>();
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
