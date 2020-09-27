using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Platforms.Android
{
    public static class Platform
    {
        public static MintPlayerMvvmConfiguration Init<TApp, TStartup>(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity app)
            where TApp : Application
            where TStartup : IStartup
        {
            var services = new ServiceCollection()
                .AddMintPlayerMvvm<TStartup>()
                .AddSingleton<TApp>()
                .BuildServiceProvider();
            var xf_app = services.GetService<TApp>();

            var loadApplicationFunc = app.GetType().GetMethod("LoadApplication", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            loadApplicationFunc.Invoke(app, new object[] { xf_app });

            return new MintPlayerMvvmConfiguration
            {
                BackButtonPressedHandler = () =>
                {
                    var navigationService = services.GetService<INavigationService>() as NavigationService;
                    var mainNavigationPage = navigationService.MainNavigation;
                    var viewModel = mainNavigationPage.CurrentPage.BindingContext as BaseViewModel;
                    var e = new BackPressedEventArgs();
                    viewModel.OnBackPressed(e);

                    return e.Handled;
                }
            };
        }
    }
}
