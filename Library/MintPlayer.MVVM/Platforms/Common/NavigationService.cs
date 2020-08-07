using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Platforms.Common
{
    public interface INavigationService
    {
        Task SetMainNavigation(INavigation navigation);
        Task SetMainPage<TViewModel>();
        Task Navigate<TViewModel>(bool modal = false);
        Task Navigate<TViewModel>(Action<TViewModel> data, bool modal = false);
        Task Pop(bool modal = false);
    }

    internal class NavigationService : INavigationService
    {
        private INavigation navigation;
        private readonly IViewModelLocator viewModelLocator;
        private readonly IServiceProvider serviceProvider;
        public NavigationService(IViewModelLocator viewModelLocator, IServiceProvider serviceProvider)
        {
            this.viewModelLocator = viewModelLocator;
            this.serviceProvider = serviceProvider;
        }

        public Task SetMainNavigation(INavigation navigation)
        {
            if (this.navigation != null)
                throw new Exception("Navigation can only be set once");

            this.navigation = navigation;
            return Task.CompletedTask;
        }

        public async Task SetMainPage<TViewModel>()
        {
            var page = PageFromVM<TViewModel>();

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
            await ((BaseViewModel)page.BindingContext).OnNavigatedTo();
        }

        public async Task Navigate<TViewModel>(bool modal = false)
        {
            var page = PageFromVM<TViewModel>();
            if (modal)
                await navigation.PushModalAsync(page);
            else
                await navigation.PushAsync(page);

            await ((BaseViewModel)page.BindingContext).OnNavigatedTo();
        }

        public async Task Navigate<TViewModel>(Action<TViewModel> data, bool modal = false)
        {
            var page = PageFromVM<TViewModel>();
            data((TViewModel)page.BindingContext);
            if (modal)
                await navigation.PushModalAsync(page);
            else
                await navigation.PushAsync(page);

            await ((BaseViewModel)page.BindingContext).OnNavigatedTo();
        }

        public async Task Pop(bool modal = false)
        {
            Page page;
            if (modal)
            {
                page = navigation.ModalStack.LastOrDefault();
                await navigation.PopModalAsync();
            }
            else
            {
                page = navigation.NavigationStack.LastOrDefault();
                await navigation.PopAsync();
            }
            await ((BaseViewModel)page.BindingContext).OnNavigatedFrom();
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
