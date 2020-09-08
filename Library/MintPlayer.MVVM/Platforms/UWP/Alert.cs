using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MintPlayer.MVVM.Platforms.UWP
{
    public static class Alert
    {
        public static async Task Show(string text)
        {
            var dialog = new MessageDialog(text);
            await dialog.ShowAsync();
        }
    }
}
