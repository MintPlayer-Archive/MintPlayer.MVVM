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
        Task SetNavigation(string regionName, INavigation navigation);
        Task SetMainPage<TViewModel>(string regionName);
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

        public Task SetNavigation(string regionName, INavigation navigation)
        {
            if (this.navigation.ContainsKey(regionName))
                throw new Exception("Navigation can only be set once");

            this.navigation[regionName] = navigation;
            return Task.CompletedTask;
        }

        public async Task SetMainPage<TViewModel>(string regionName)
        {
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var navigation = this.navigation[regionName];
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
            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(null);
        }

        private async Task InternalNavigate<TViewModel>(string regionName, NavigationParameters parameters, Action<TViewModel> data, bool modal) where TViewModel : BaseViewModel
        {
            if (!navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var viewModel = (TViewModel)page.BindingContext;
            viewModel.IsModal = modal;
            data(viewModel);

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
            var view = (Page)Activator.CreateInstance(viewType);
            view.BindingContext = ActivatorUtilities.CreateInstance<TViewModel>(serviceProvider);
            return view;
        }
    }
}
