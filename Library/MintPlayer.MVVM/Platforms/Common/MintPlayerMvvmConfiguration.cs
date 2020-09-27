using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.MVVM.Platforms.Common
{
    public class MintPlayerMvvmConfiguration
    {
        public Func<bool> BackButtonPressedHandler { get; internal set; }
    }
}
