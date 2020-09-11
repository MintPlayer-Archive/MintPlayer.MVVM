using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Platforms.Android
{
    public static class Platform
    {
        public static void Init<TApp, TStartup>(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity app)
            where TApp : Xamarin.Forms.Application
            where TStartup : IStartup
        {
            var services = new ServiceCollection()
                .AddMintPlayerMvvm<TStartup>()
                .AddSingleton<TApp>()
                .BuildServiceProvider();
            var xf_app = services.GetService<TApp>();

            var loadApplicationFunc = app.GetType().GetMethod("LoadApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            loadApplicationFunc.Invoke(app, new object[] { xf_app });
        }
    }
}
