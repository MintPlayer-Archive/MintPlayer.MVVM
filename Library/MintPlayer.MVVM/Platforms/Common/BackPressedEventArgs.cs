using System;

namespace MintPlayer.MVVM.Platforms.Common
{
    public class BackPressedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
    }
}
