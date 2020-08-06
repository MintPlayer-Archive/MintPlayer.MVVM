using MintPlayer.MVVM.Demo.Models;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class ItemDetailVM : BaseVM
    {
        public ItemDetailVM()
        {
            Item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };
        }

        public Item Item { get; set; }
    }
}
