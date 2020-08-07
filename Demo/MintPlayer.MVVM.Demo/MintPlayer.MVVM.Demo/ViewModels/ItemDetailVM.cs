using MintPlayer.MVVM.Demo.Models;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemDetailVM : BaseVM
    {
        public ItemDetailVM()
        {
            Item = new Artist
            {
                Id = 0,
                Name = "New artist",
                YearStarted = null,
                YearQuit = null
            };
        }

        public Artist Item { get; set; }
    }
}
