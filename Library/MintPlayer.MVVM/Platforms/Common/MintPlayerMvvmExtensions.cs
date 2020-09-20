using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common.Events;
using MintPlayer.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MintPlayer.MVVM.Platforms.Common
{
    internal static class MintPlayerMvvmExtensions
    {
        internal static IServiceCollection AddMintPlayerMvvm<TStartup>(this IServiceCollection services) where TStartup : IStartup
        {
            var tstartup = typeof(TStartup);
            if (tstartup.GetInterfaces(true).Contains(typeof(IStartup)))
            {
                const string message = "You must pass the platform-specific Startup class into the MintPlayer.MVVM.Platform.Init method";
                Console.WriteLine(message);
                throw new Exception(message);
            }

            // Setup configuration
            var configurationBuilder = new ConfigurationBuilder();

            #region Add JSON configuration
            // Xamarin.Forms appsettings
            var appSettingsName = $"{tstartup.BaseType.Assembly.GetName().Name}.appsettings.json";
            configurationBuilder.AddJsonConfiguration(tstartup.BaseType.Assembly, appSettingsName);

            // Android appsettings
            var appSettingsPlatformName = tstartup.Assembly.GetManifestResourceNames().FirstOrDefault(n => n.ToLower().EndsWith(".appsettings.json"));
            configurationBuilder.AddJsonConfiguration(tstartup.Assembly, appSettingsPlatformName);

            //if (!string.IsNullOrEmpty(env.EnvironmentName))
            //{
            //    // Xamarin.Forms Environment appsettings
            //    var appSettingsNameEnv = $"{tstartup.BaseType.Assembly.GetName().Name}.appsettings.{env.EnvironmentName}.json";
            //    configurationBuilder.AddJsonConfiguration(tstartup.BaseType.Assembly, appSettingsNameEnv);

            //    // Android Environment appsettings
            //    var appSettingsPlatformNameEnv = tstartup.Assembly.GetManifestResourceNames().FirstOrDefault(n => n.ToLower().EndsWith($".appsettings.{env.EnvironmentName}.json"));
            //    configurationBuilder.AddJsonConfiguration(tstartup.Assembly, appSettingsPlatformNameEnv);
            //}
            #endregion

            var configuration = configurationBuilder.Build();

            // Setup Dependency Injection
            var serviceCollection = services
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IEventAggregator, EventAggregator>()
                .AddSingleton<IViewModelLocator>(new ViewModelLocator(tstartup));

            // Create an instance of the startup class
            var startup = (TStartup)tstartup.GetConstructor(new[] { typeof(IConfiguration) }).Invoke(new object[] { configuration });
            startup.ConfigureServices(serviceCollection);

            return serviceCollection;
        }

        private static void AddJsonConfiguration(this ConfigurationBuilder builder, Assembly configAssembly, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var stream = configAssembly.GetManifestResourceStream(filename);
                if (stream != null)
                    builder.AddJsonStream(stream);
            }
        }
    }
}
