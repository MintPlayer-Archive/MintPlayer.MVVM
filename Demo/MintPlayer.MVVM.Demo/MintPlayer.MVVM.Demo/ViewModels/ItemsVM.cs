using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.Views;
using System.Windows.Input;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemsVM : BaseVM
    {
        private readonly INavigationService navigationService;
        public ItemsVM(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(OnLoadItems);
            AddItemCommand = new Command(OnAddItem);

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        #region Bindings
        public ObservableCollection<Item> Items { get; set; }
        #endregion

        #region Commands
        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        #endregion

        #region Methods
        private async void OnLoadItems()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void OnAddItem(object parameter)
        {
            await navigationService.NavigateAsync()
        }
        #endregion
    }
}