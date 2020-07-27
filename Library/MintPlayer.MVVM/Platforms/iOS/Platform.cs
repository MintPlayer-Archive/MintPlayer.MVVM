using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.iOS
{
    public static class Platform
    {
        public static TApp Init<TApp>(global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate appDelegate) where TApp : Xamarin.Forms.Application
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
