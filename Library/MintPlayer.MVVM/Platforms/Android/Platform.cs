using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.Android
{
    public static class Platform
    {
        public static TApp Init<TApp>(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity app) where TApp : Xamarin.Forms.Application
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
