using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class BaseVM : BaseViewModel
    {
        bool isBusy = true;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
