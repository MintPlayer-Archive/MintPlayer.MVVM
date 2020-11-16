using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MintPlayer.MVVM.Platforms.Common.Exceptions;
using MintPlayer.MVVM.Platforms.Common.Options;

namespace MintPlayer.MVVM.Platforms.Common
{
    public interface INavigationService
    {
        Task SetNavigation(string regionName, INavigation navigation, NavigationOptions navOptions);
        Task<Page> SetMainPage<TViewModel>(string regionName);
        Task Navigate<TViewModel>(string regionName, bool modal = false) where TViewModel : BaseViewModel;
        Task Navigate<TViewModel>(string regionName, Action<TViewModel> data, bool modal = false) where TViewModel : BaseViewModel;
        Task Navigate<TViewModel>(string regionName, NavigationParameters parameters, bool modal = false) where TViewModel : BaseViewModel;
        Task Pop(string regionName, bool modal = false);
    }

    internal class NavigationService : INavigationService
    {
        private readonly IViewModelLocator viewModelLocator;
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<string, INavigation> navigation;
        public NavigationService(IViewModelLocator viewModelLocator, IServiceProvider serviceProvider)
        {
            this.viewModelLocator = viewModelLocator;
            this.serviceProvider = serviceProvider;
            this.navigation = new Dictionary<string, INavigation>();
        }

        public async Task<Page> SetMainPage<TViewModel>(string regionName)
        {
            // Region name must exist.
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            // Create page from the viewmodel.
            var page = PageFromVM<TViewModel>();
            var navigation = this.navigation[regionName];

            // If necessary, remove all pages in order to set the mainpage as only page on this stack.
            var firstPage = navigation.NavigationStack.FirstOrDefault();
            if (firstPage == null)
            {
                await navigation.PushAsync(page);
            }
            else
            {
                navigation.InsertPageBefore(page, firstPage);
                await navigation.PopToRootAsync();
            }

            // Invoke the OnNavigatedTo hook.
            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(null);

            return page;
        }

        public async Task SetNavigation(string regionName, INavigation navigation, NavigationOptions navOptions)
        {
            if (this.navigation.ContainsKey(regionName))
                throw new Exception("Navigation can only be set once");

            // Keep navigation stack in dictionary
            this.navigation[regionName] = navigation;

            // Call await this.SetMainPage<TViewModel>(regionName);
            var setMainPageMethod = GetType().GetMethod(nameof(SetMainPage)).MakeGenericMethod(navOptions.MainViewModel);
            var task = (Task<Page>)setMainPageMethod.Invoke(this, new object[] { regionName });
            var mainPage = await task;

            // NavigationPage options
            NavigationPage.SetHasBackButton(mainPage, navOptions.HasBackButton);
            NavigationPage.SetHasNavigationBar(mainPage, navOptions.HasNavigationBar);

            var navType = navigation.GetType();
            if (navType.Name == "NavigationImpl")
            {
                var ownerProperty = navType.GetProperty("Owner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var navigationPage = (NavigationPage)ownerProperty.GetValue(navigation);
                navigationPage.Pushed += (sender, e) =>
                {
                };
                navigationPage.Popped += (sender, e) =>
                {
                };
            }
        }

        private async Task InternalNavigate<TViewModel>(string regionName, NavigationParameters parameters, Action<TViewModel> data, bool modal) where TViewModel : BaseViewModel
        {
            if (!navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var viewModel = (TViewModel)page.BindingContext;
            viewModel.IsModal = modal;
            if (data != null) data(viewModel);

            if (modal)
                await navigation[regionName].PushModalAsync(new NavigationPage(page));
            else
                await navigation[regionName].PushAsync(page);

            await viewModel.OnNavigatedTo(parameters);
        }

        public async Task Navigate<TViewModel>(string regionName, bool modal = false) where TViewModel : BaseViewModel
        {
            await InternalNavigate<TViewModel>(regionName, null, null, modal);
        }

        public async Task Navigate<TViewModel>(string regionName, Action<TViewModel> data, bool modal = false) where TViewModel : BaseViewModel
        {
            await InternalNavigate<TViewModel>(regionName, null, data, modal);
        }

        public async Task Navigate<TViewModel>(string regionName, NavigationParameters parameters, bool modal = false) where TViewModel : BaseViewModel
        {
            await InternalNavigate<TViewModel>(regionName, parameters, null, modal);
        }

        public async Task Pop(string regionName, bool modal = false)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var navigation = this.navigation[regionName];
            if (modal)
            {
                var page = (NavigationPage)navigation.ModalStack.LastOrDefault();
                await navigation.PopModalAsync();
                await ((BaseViewModel)page.RootPage.BindingContext).OnNavigatedFrom();
            }
            else
            {
                var page = navigation.NavigationStack.LastOrDefault();
                await navigation.PopAsync();
                await ((BaseViewModel)page.BindingContext).OnNavigatedFrom();
            }
        }

        private Page PageFromVM<TViewModel>()
        {
            var viewType = viewModelLocator.Resolve<TViewModel>();
            var view = (Page)ActivatorUtilities.CreateInstance(serviceProvider, viewType);
            view.BindingContext = ActivatorUtilities.CreateInstance<TViewModel>(serviceProvider);
            return view;
        }
    }
}
