using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.Views;
using System.Windows.Input;
using MintPlayer.MVVM.Platforms.Common;
using System.Threading.Tasks;
using MintPlayer.MVVM.Demo.Services;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemsVM : BaseVM
    {
        private readonly INavigationService navigationService;
        private readonly IArtistService artistService;
        public ItemsVM(INavigationService navigationService, IArtistService artistService)
        {
            this.navigationService = navigationService;
            this.artistService = artistService;
            Title = "Browse";
            Items = new MintPlayer.ObservableCollection.ObservableCollection<Artist>();
            LoadItemsCommand = new Command(OnLoadItems);
            AddItemCommand = new Command(OnAddItem);

            MessagingCenter.Subscribe<NewItemPage, Artist>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item;
                Items.Add(newItem);
                //await DataStore.AddItemAsync(newItem);
            });
        }

        #region Bindings
        public MintPlayer.ObservableCollection.ObservableCollection<Artist> Items { get; set; }
        #endregion

        #region Commands
        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        #endregion

        #region Methods
        private async void OnLoadItems()
        {
            await ReloadItems();
        }

        private async void OnAddItem(object parameter)
        {
            await navigationService.Navigate<NewItemVM>();
        }

        protected override async Task OnNavigatedTo()
        {
            if (Items.Count == 0)
            {
                await ReloadItems();
            }
        }

        private async Task ReloadItems()
        {
            try
            {
                IsBusy = true;

                Items.Clear();
                var items = await artistService.GetArtists();
                Items.AddRange(items);
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
        #endregion
    }
}