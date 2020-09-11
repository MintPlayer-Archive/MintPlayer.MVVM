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
using MintPlayer.MVVM.Platforms.Common.Events;
using MintPlayer.MVVM.Demo.Events;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemsVM : BaseVM
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        private readonly IArtistService artistService;
        public ItemsVM(INavigationService navigationService, IEventAggregator eventAggregator, IArtistService artistService)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.artistService = artistService;

            Title = "Browse";
            Artists = new ObservableCollection<Artist>();
            LoadItemsCommand = new Command(OnLoadItems);
            AddItemCommand = new Command(OnAddItem);
            SelectArtistCommand = new Command(OnSelectArtist);

            //MessagingCenter.Subscribe<NewItemPage, Artist>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item;
            //    Artists.Add(newItem);
            //    //await DataStore.AddItemAsync(newItem);
            //});
        }

        #region Bindings
        public ObservableCollection<Artist> Artists { get; set; }
        #region SelectedItem
        private Artist selectedArtist;
        public Artist SelectedArtist
        {
            get => selectedArtist;
            set
            {
                selectedArtist = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Commands
        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand SelectArtistCommand { get; set; }
        #endregion

        #region Methods
        private async void OnLoadItems()
        {
            await ReloadItems();
        }

        private async void OnAddItem(object parameter)
        {
            await navigationService.Navigate<NewItemVM>(true);
        }

        protected override async Task OnNavigatedTo(NavigationParameters parameters)
        {
            eventAggregator.GetEvent<ItemCreatedEvent>().Subscribe(OnItemCreated);
            if (Artists.Count == 0)
            {
                await ReloadItems();
            }
        }

        protected override Task OnNavigatedFrom()
        {
            eventAggregator.GetEvent<ItemCreatedEvent>().Unsubscribe(OnItemCreated);
            return Task.CompletedTask;
        }

        private async Task ReloadItems()
        {
            try
            {
                IsBusy = true;

                Artists.Clear();
                var items = await artistService.GetArtists();
                //Items.AddRange(items);
                foreach (var item in items)
                    Artists.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await Task.Delay(50);
                IsBusy = false;
            }
        }

        private void OnSelectArtist(object obj)
        {
            var artist = obj as Artist;
            if (artist != null)
            {
                //navigationService.Navigate<ItemDetailVM>((model) => { model.Artist = obj as Artist; });
                navigationService.Navigate<ItemDetailVM>(new NavigationParameters { { "ArtistId", artist.Id } });
                SelectedArtist = null;
            }
        }

        private void OnItemCreated(object item)
        {
            if (item is Artist artist)
            {
                Artists.Add(artist);
            }
        }
        #endregion
    }
}