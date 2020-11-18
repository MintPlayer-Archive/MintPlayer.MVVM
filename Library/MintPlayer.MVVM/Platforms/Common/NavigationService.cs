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
        Task ClearNavigation(string regionName);
        Task<Page> SetMainPage<TViewModel>(string regionName) where TViewModel : BaseViewModel;
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

        public async Task<Page> SetMainPage<TViewModel>(string regionName) where TViewModel : BaseViewModel
        {
            // Region name must exist.
            if (!this.navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            // Create page from the viewmodel.
            var page = PageFromVM<TViewModel>();
            var viewModel = (TViewModel)page.BindingContext;
            var navigation = this.navigation[regionName];

            // If necessary, remove all pages in order to set the mainpage as only page on this stack.
            await OnWireEvents(page);
            var firstPage = navigation.NavigationStack.FirstOrDefault();
            viewModel.isPushing = true;
            if (firstPage == null)
            {
                await navigation.PushAsync(page);
            }
            else
            {
                navigation.InsertPageBefore(page, firstPage);
                await navigation.PopToRootAsync();
            }
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                viewModel.isPushing = false;
            });

            await OnBindingContextNavigatedTo(page, null);

            return page;
        }

        public async Task SetNavigation(string regionName, INavigation navigation, NavigationOptions navOptions)
        {
            //if (this.navigation.ContainsKey(regionName))
            //    throw new Exception("Navigation can only be set once");

            // Keep navigation stack in dictionary
            this.navigation[regionName] = navigation;

            // Call await this.SetMainPage<TViewModel>(regionName);
            var setMainPageMethod = GetType().GetMethod(nameof(SetMainPage)).MakeGenericMethod(navOptions.MainViewModel);
            var task = (Task<Page>)setMainPageMethod.Invoke(this, new object[] { regionName });
            var mainPage = await task;

            // NavigationPage options
            NavigationPage.SetHasBackButton(mainPage, navOptions.HasBackButton);
            NavigationPage.SetHasNavigationBar(mainPage, navOptions.HasNavigationBar);
        }

        public Task ClearNavigation(string regionName)
        {
            if (navigation.ContainsKey(regionName))
            {
                navigation.Remove(regionName);
            }
            return Task.CompletedTask;
        }

        #region Appearing/Disappearing
        private async Task OnBindingContextNavigatedTo(Page page, NavigationParameters parameters)
        {
            // Invoke the OnNavigatedTo hook.
            await ((BaseViewModel)page.BindingContext).OnNavigatedTo(parameters);
        }

        private Task OnWireEvents(Page page)
        {
            // Wire the Appearing/Disappearing events
            page.Appearing += Page_Appearing;
            page.Disappearing += Page_Disappearing;

            return Task.CompletedTask;
        }

        private async Task OnBindingContextNavigatedFrom(Page page)
        {
            var bindingContext = (BaseViewModel)page.BindingContext;
            await bindingContext.OnNavigatedFrom();
        }

        private Task OnUnwireEvents(Page page)
        {
            // Unwire the Appearing/Disappearing events
            page.Appearing -= Page_Appearing;
            page.Disappearing -= Page_Disappearing;

            return Task.CompletedTask;
        }

        private void Page_Appearing(object sender, EventArgs e)
        {
            if (sender is Page page)
            {
                var bindingContext = (BaseViewModel)page.BindingContext;
                bindingContext.OnAppearing(bindingContext.isPushing);
            }
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            if (sender is Page page)
            {
                var bindingContext = (BaseViewModel)page.BindingContext;
                bindingContext.OnDisappearing(bindingContext.isPopping);
            }
        }
        #endregion

        private async Task InternalNavigate<TViewModel>(string regionName, NavigationParameters parameters, Action<TViewModel> data, bool modal) where TViewModel : BaseViewModel
        {
            if (!navigation.ContainsKey(regionName))
                throw new RegionNotFoundException(regionName);

            var page = PageFromVM<TViewModel>();
            var viewModel = (TViewModel)page.BindingContext;
            viewModel.IsModal = modal;
            if (data != null) data(viewModel);

            await OnWireEvents(page);

            viewModel.isPushing = true;
            if (modal)
                await navigation[regionName].PushModalAsync(new NavigationPage(page));
            else
                await navigation[regionName].PushAsync(page);
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                viewModel.isPushing = false;
            });

            await OnBindingContextNavigatedTo(page, parameters);
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
                var viewModel = (BaseViewModel)page.BindingContext;
                viewModel.isPopping = true;
                await navigation.PopModalAsync();
                viewModel.isPopping = false;
                await OnBindingContextNavigatedFrom(page);
                await OnUnwireEvents(page);
            }
            else
            {
                var page = navigation.NavigationStack.LastOrDefault();
                var viewModel = (BaseViewModel)page.BindingContext;
                viewModel.isPopping = true;
                await navigation.PopAsync();
                viewModel.isPopping = false;
                await OnBindingContextNavigatedFrom(page);
                await OnUnwireEvents(page);
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
