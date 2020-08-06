using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class AboutVM : BaseVM
    {
        public AboutVM()
        {
            Title = "About";
            OpenWebCommand = new Command(OnOpenWeb);
        }

        public ICommand OpenWebCommand { get; }

        private async void OnOpenWeb(object parameter)
        {
            await Browser.OpenAsync("https://xamarin.com");
        }
    }
}