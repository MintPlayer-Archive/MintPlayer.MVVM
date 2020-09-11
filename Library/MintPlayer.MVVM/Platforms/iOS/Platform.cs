using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.iOS
{
    public static class Platform
    {
        public static void Init<TApp, TStartup>(global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate appDelegate)
            where TApp : Xamarin.Forms.Application
            where TStartup : IStartup
        {
            var services = new ServiceCollection()
                .AddMintPlayerMvvm<TStartup>()
                .AddSingleton<TApp>()
                .BuildServiceProvider();
            var xf_app = services.GetService<TApp>();


            var loadApplicationFunc = appDelegate.GetType().GetMethod("LoadApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            loadApplicationFunc.Invoke(appDelegate, new object[] { xf_app });


            //return xf_app;
        }
    }
}
