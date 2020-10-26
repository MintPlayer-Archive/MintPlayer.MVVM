using System;
using Xamarin.Forms;
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
            var mainPage = new MainPage
            {
                BindingContext = ActivatorUtilities.CreateInstance<MainVM>(serviceProvider)
            };
            MainPage = mainPage;
            navigationService.SetNavigation(
                RegionNames.MainRegion,
                mainPage.Detail.Navigation,
                new Platforms.Common.Options.NavigationOptions
                {
                    MainViewModel = typeof(ItemsVM)
                }
            );
            navigationService.SetNavigation(
                RegionNames.MenuRegion,
                mainPage.Master.Navigation,
                new Platforms.Common.Options.NavigationOptions
                {
                    MainViewModel = typeof(MenuVM),
                    HasNavigationBar = false
                }
            );
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
