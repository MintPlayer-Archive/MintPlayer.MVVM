using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MintPlayer.MVVM.Demo.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var services = new ServiceCollection()
                .AddMintPlayerMvvm()
                .AddSingleton<Demo.App>()
                .BuildServiceProvider();
            var xf_app = services.GetService<Demo.App>();

            LoadApplication(xf_app);
        }
    }
}
