using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.UAP
{
    public static class Platform
    {
        public static TApp Init<TApp>(global::Xamarin.Forms.Platform.UWP.WindowsPage mainPage) where TApp : Xamarin.Forms.Application
        {
            var services = new ServiceCollection()
                .AddMintPlayerMvvm()
                .AddSingleton<TApp>()
                .BuildServiceProvider();
            var xf_app = services.GetService<TApp>();

            return xf_app;
        }
    }
}
