using MintPlayer.MVVM.Demo.Models;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Platforms.Common;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemDetailVM : BaseVM
    {
        private readonly IArtistService artistService;
        public ItemDetailVM(IArtistService artistService)
        {
            this.artistService = artistService;
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

        protected override async Task OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
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
    }
}
