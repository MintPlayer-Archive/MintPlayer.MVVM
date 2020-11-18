using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemDetailVM : BaseVM
    {
        private readonly IArtistService artistService;
        private readonly INavigationService navigationService;
        public ItemDetailVM(IArtistService artistService, INavigationService navigationService)
        {
            this.artistService = artistService;
            this.navigationService = navigationService;
            SelectSongCommand = new Command(OnSelectSong, CanSelectSong);
        }

        #region Artist
        private Artist artist;
        public Artist Artist
        {
            get => artist;
            set
            {
                artist = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand SelectSongCommand { get; set; }
        #endregion

        protected override async Task OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Navigated to {GetType().Name}");
                IsBusy = true;
                await Task.Delay(1000);
                var artist = await artistService.GetArtist(parameters.GetValue<int>("ArtistId"), true);
                Artist = artist;
            }
            catch (System.Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override Task OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"Navigated from {GetType().Name}");
            return Task.CompletedTask;
        }

        protected override Task OnAppearing(bool pushed)
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} appearing ({pushed})");
            return Task.CompletedTask;
        }

        protected override Task OnDisappearing(bool popped)
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} disappearing ({popped})");
            return Task.CompletedTask;
        }

        private async void OnSelectSong(object obj)
        {
            await navigationService.Navigate<Song.SongShowVM>(Constants.RegionNames.MainRegion);
        }

        private bool CanSelectSong(object arg) => true;
    }
}
