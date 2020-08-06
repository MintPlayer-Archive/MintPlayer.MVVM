using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Platforms.Common
{
    public interface INavigationService
    {
        Task NavigateAsync<TViewModel>();
    }

    internal class NavigationService : INavigationService
    {
        private readonly IViewModelLocator viewModelLocator;
        private readonly IServiceProvider serviceProvider;
        public NavigationService(IViewModelLocator viewModelLocator, IServiceProvider serviceProvider)
        {
            this.viewModelLocator = viewModelLocator;
            this.serviceProvider = serviceProvider;
        }

        public Task NavigateAsync<TViewModel>()
        {
            var viewModel = ActivatorUtilities.CreateInstance<TViewModel>(serviceProvider);
            var viewType = viewModelLocator.Resolve<TViewModel>();
            var view = Activator.CreateInstance(viewType, viewModel);


        }
    }
}
