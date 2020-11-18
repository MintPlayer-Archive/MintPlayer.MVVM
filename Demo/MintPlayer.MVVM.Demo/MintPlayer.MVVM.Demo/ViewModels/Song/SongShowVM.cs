using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Demo.ViewModels.Song
{
    public class SongShowVM : BaseVM
    {
        public SongShowVM()
        {

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
