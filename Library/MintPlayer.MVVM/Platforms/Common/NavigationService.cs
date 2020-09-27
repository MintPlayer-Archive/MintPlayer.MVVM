using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MintPlayer.MVVM.Platforms.Common.Exceptions;

namespace MintPlayer.MVVM.Platforms.Common
{
    public interface INavigationService
    {
        Task SetNavigation(string regionName, NavigationPage navigation, bool isMainNavigation = false);
        Task SetMainPage<TViewModel>(string regionName);
        Task Navigate<TViewModel>(string regionName, bool modal = false);
        Task Navigate<TViewModel>(string regionName, Action<TViewModel> data, bool modal = false);
        Task Navigate<TViewModel>(string regionName, NavigationParameters parameters, bool modal = false);
        Task Pop(string regionName, bool modal = false);
    }

    internal class NavigationService : INavigationService
    {
        private readonly IViewModelLocator viewModelLocator;
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<string, NavigationPage> navigation;

        internal NavigationPage MainNavigation { get; private set; }
        
        public NavigationService(IViewModelLocator viewModelLocator, IServiceProvider serviceProvider)
        {
            this.viewModelLocator = viewModelLocator;
            this.serviceProvider = serviceProvider;
            this.navigation = new Dictionary<string, NavigationPage>();
        }

        public Task SetNavigation(string regionName, NavigationPage navigation, bool isMainNavigation = false)
        {
            if (this.navigation.ContainsKey(regionName))
                throw new Exception("Navigation can only be set once");

            this.navigation[regionName] = navigation;
            if (isMainNavigation && (MainNavigation == null))
            {
                MainNavigation = navigation;
            }

            return Task.CompletedTask;
        }

        public async Task SetMainPage<TViewModel>(string regionName)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var navigationPage = this.navigation[regionName];
            var firstPage = navigationPage.CurrentPage;
            if (firstPage == null)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                navigationPage.Navigation.InsertPageBefore(page, firstPage);
                await navigationPage.PopToRootAsync();
            }
            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(null);
        }

        public async Task Navigate<TViewModel>(string regionName, bool modal = false)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var navigationPage = this.navigation[regionName];
            if (modal)
                await navigationPage.Navigation.PushModalAsync(new NavigationPage(page));
            else
                await navigationPage.Navigation.PushAsync(page);

            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(null);
        }

        public async Task Navigate<TViewModel>(string regionName, Action<TViewModel> data, bool modal = false)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var navigationPage = this.navigation[regionName];
            data((TViewModel)page.BindingContext);
            if (modal)
                await navigationPage.Navigation.PushModalAsync(page);
            else
                await navigationPage.Navigation.PushAsync(page);

            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(null);
        }

        public async Task Navigate<TViewModel>(string regionName, NavigationParameters parameters, bool modal = false)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var navigationPage = this.navigation[regionName];
            if (modal)
                await navigationPage.Navigation.PushModalAsync(page);
            else
                await navigationPage.Navigation.PushAsync(page);

            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(parameters);
        }

        public async Task Pop(string regionName, bool modal = false)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var navigationPage = this.navigation[regionName];
            if (modal)
            {
                var page = (NavigationPage)navigationPage.Navigation.ModalStack.LastOrDefault();
                await navigationPage.Navigation.PopModalAsync();
                await ((BaseViewModel)page.RootPage.BindingContext).OnNavigatedFrom();
            }
            else
            {
                var page = navigationPage.Navigation.NavigationStack.LastOrDefault();
                await navigationPage.PopAsync();
                await ((BaseViewModel)page.BindingContext).OnNavigatedFrom();
            }
        }

        private Page PageFromVM<TViewModel>()
        {
            var viewType = viewModelLocator.Resolve<TViewModel>();
            var view = (Page)Activator.CreateInstance(viewType);
            view.BindingContext = ActivatorUtilities.CreateInstance<TViewModel>(serviceProvider);
            return view;
        }
    }
}
