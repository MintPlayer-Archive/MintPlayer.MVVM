using System;

namespace MintPlayer.MVVM.Platforms.Common.Options
{
    public class NavigationOptions
    {
        public bool HasNavigationBar { get; set; } = true;
        public bool HasBackButton { get; set; } = true;
        public Type MainViewModel { get; set; }
    }
}
