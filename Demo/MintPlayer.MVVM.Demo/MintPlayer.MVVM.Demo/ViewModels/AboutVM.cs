using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Threading.Tasks;
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

        protected override Task OnNavigatedTo(NavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"Navigated to {GetType().Name}");
            return Task.CompletedTask;
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
    }
}