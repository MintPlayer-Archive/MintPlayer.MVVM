using MintPlayer.MVVM.Demo.Models;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemDetailVM : BaseVM
    {
        public ItemDetailVM()
        {
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
    }
}
