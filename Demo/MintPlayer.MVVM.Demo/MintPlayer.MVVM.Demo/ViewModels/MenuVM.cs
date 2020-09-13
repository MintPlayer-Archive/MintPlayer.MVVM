using MintPlayer.MVVM.Demo.Constants;
using MintPlayer.MVVM.Demo.Events;
using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Platforms.Common.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class MenuVM : BaseVM
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        public MenuVM(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;

            MenuItems = new ObservableCollection<HomeMenuItem>();
            MenuItemSelectedCommand = new Command(OnMenuItemSelected);
        }

        #region Bindings
        #region MenuItems
        private ObservableCollection<HomeMenuItem> menuItems;
        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get => menuItems;
            set => SetProperty(ref menuItems, value);
        }
        #endregion
        #endregion
        #region Commands
        public ICommand MenuItemSelectedCommand { get; set; }
        #endregion
        #region Methods
        private void OnMenuItemSelected(object parameter)
        {
            if (parameter is SelectedItemChangedEventArgs e)
            {
                if (e.SelectedItem is HomeMenuItem item)
                {
                    switch (item.Id)
                    {
                        case MenuItemType.Browse:
                            navigationService.SetMainPage<ItemsVM>(RegionNames.MainRegion);
                            break;
                        case MenuItemType.About:
                            navigationService.SetMainPage<AboutVM>(RegionNames.MainRegion);
                            break;
                        default:
                            throw new Exception("Invalid page type");
                    }
                    eventAggregator.GetEvent<MenuItemSelectedEvent>().Publish(item.Id);
                }
            }
        }
        protected override Task OnNavigatedTo(NavigationParameters parameters)
        {
            MenuItems.Add(new HomeMenuItem { Id = MenuItemType.Browse, Title = "Browse" });
            MenuItems.Add(new HomeMenuItem { Id = MenuItemType.About, Title = "About" });
            return Task.CompletedTask;
        }
        #endregion
    }
}
