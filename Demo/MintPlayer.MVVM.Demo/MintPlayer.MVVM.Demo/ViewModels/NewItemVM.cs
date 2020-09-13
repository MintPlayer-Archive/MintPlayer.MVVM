using MintPlayer.MVVM.Demo.Constants;
using MintPlayer.MVVM.Demo.Events;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Platforms.Common.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class NewItemVM : BaseVM
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;

        public NewItemVM(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;

            Artist = new Models.Artist
            {
                Id = 0,
                Name = string.Empty,
                YearStarted = null,
                YearQuit = null
            };
            SaveCommand = new Command(OnSave, CanSave);
            CancelCommand = new Command(OnCancel, CanCancel);
        }

        #region Bindings
        public Models.Artist Artist { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Methods
        private async void OnSave(object obj)
        {
            eventAggregator.GetEvent<ItemCreatedEvent>().Publish(Artist);
            await navigationService.Pop(RegionNames.MainRegion, true);
        }

        private bool CanSave(object arg)
        {
            return true;
        }

        private async void OnCancel(object obj)
        {
            await navigationService.Pop(RegionNames.MainRegion, true);
        }

        private bool CanCancel(object arg)
        {
            return true;
        }
        #endregion
    }
}
