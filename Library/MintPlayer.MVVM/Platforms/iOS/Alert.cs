using System.Threading.Tasks;
using UIKit;

namespace MintPlayer.MVVM.Platforms.iOS
{
    public static class Alert
    {
        public static async Task Show(UIViewController viewController, string text)
        {
            //Create Alert
            var okAlertController = UIAlertController.Create("Title", text, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            await viewController.PresentViewControllerAsync(okAlertController, true);
        }
    }
}
