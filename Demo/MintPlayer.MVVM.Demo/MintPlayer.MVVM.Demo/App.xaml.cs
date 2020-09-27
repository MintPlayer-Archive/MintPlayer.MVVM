using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Demo.Views;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Demo.ViewModels;
using MintPlayer.MVVM.Demo.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace MintPlayer.MVVM.Demo
{
    public partial class App : Application
    {
        private readonly INavigationService navigationService;
        private readonly IServiceProvider serviceProvider;
        public App(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            this.navigationService = navigationService;
            this.serviceProvider = serviceProvider;
            InitializeComponent();
            MainPage = new MainPage
            {
                BindingContext = ActivatorUtilities.CreateInstance<MainVM>(serviceProvider)
            };

            navigationService.SetNavigation(RegionNames.MainRegion, (NavigationPage)((MasterDetailPage)MainPage).Detail, true);
            navigationService.SetMainPage<ItemsVM>(RegionNames.MainRegion);
            navigationService.SetNavigation(RegionNames.MenuRegion, (NavigationPage)((MasterDetailPage)MainPage).Master);
            navigationService.SetMainPage<MenuVM>(RegionNames.MenuRegion);
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
