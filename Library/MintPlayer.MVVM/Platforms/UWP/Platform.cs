using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.UWP
{
    public static class Platform
    {
        public static void Init<TApp, TStartup>(global::Xamarin.Forms.Platform.UWP.WindowsPage mainPage)
            where TApp : Xamarin.Forms.Application
            where TStartup : IStartup
        {
            var services = new ServiceCollection()
                .AddMintPlayerMvvm<TStartup>()
                .AddSingleton<TApp>()
                .BuildServiceProvider();
            var xf_app = services.GetService<TApp>();

            var loadApplicationFunc = mainPage.GetType().GetMethod("LoadApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            loadApplicationFunc.Invoke(mainPage, new object[] { xf_app });

            //return xf_app;
        }
    }
}
