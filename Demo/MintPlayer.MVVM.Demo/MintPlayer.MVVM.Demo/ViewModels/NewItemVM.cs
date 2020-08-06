using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class NewItemVM : BaseVM
    {
        private readonly INavigationService navigationService;
        public NewItemVM(INavigationService navigationService)
        {
            Item = new Models.Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            SaveCommand = new Command(OnSave, CanSave);
            CancelCommand = new Command(OnCancel, CanCancel);
            this.navigationService = navigationService;
        }

        #region Bindings
        public Models.Item Item { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Methods
        private void OnSave(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSave(object arg)
        {
            return true;
        }

        private void OnCancel(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanCancel(object arg)
        {
            return true;
        }
        #endregion
    }
}
