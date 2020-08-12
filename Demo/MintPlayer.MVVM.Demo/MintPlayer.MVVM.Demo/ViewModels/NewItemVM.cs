using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class NewItemVM : BaseVM
    {
        private readonly INavigationService navigationService;
        public NewItemVM(INavigationService navigationService)
        {
            Item = new Models.Artist
            {
                Id = 0,
                Name = "New artist",
                YearStarted = null,
                YearQuit = null
            };

            SaveCommand = new Command(OnSave, CanSave);
            CancelCommand = new Command(OnCancel, CanCancel);
            this.navigationService = navigationService;
        }

        #region Bindings
        public Models.Artist Item { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Methods
        private async void OnSave(object obj)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await navigationService.Pop(true);
        }

        private bool CanSave(object arg)
        {
            return true;
        }

        private async void OnCancel(object obj)
        {
            await navigationService.Pop(true);
        }

        private bool CanCancel(object arg)
        {
            return true;
        }
        #endregion
    }
}
