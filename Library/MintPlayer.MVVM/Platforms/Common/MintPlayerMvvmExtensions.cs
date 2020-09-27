using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Platforms.Common.EventAggregator;
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

            var appSettingsName = $"{tstartup.BaseType.Assembly.GetName().Name}.appsettings.json";
            var appSettingsXamarinFormsStream = tstartup.BaseType.Assembly.GetManifestResourceStream(appSettingsName);

            // Read environment
            var env = GetEnvironmentFromAppSettings(appSettingsXamarinFormsStream);

            // Xamarin.Forms appsettings
            configurationBuilder.AddJsonConfiguration(tstartup.BaseType.Assembly, appSettingsName);

            // Android appsettings
            var appSettingsPlatformName = tstartup.Assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(".appsettings.json"));
            configurationBuilder.AddJsonConfiguration(tstartup.Assembly, appSettingsPlatformName);

            // Xamarin.Forms Environment appsettings
            var appSettingsNameEnv = $"{tstartup.BaseType.Assembly.GetName().Name}.appsettings.{env}.json";
            configurationBuilder.AddJsonConfiguration(tstartup.BaseType.Assembly, appSettingsNameEnv);

            // Android Environment appsettings
            var appSettingsPlatformNameEnv = tstartup.Assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith($".appsettings.{env}.json"));
            configurationBuilder.AddJsonConfiguration(tstartup.Assembly, appSettingsPlatformNameEnv);

            #endregion

            var configuration = configurationBuilder.Build();

            // Setup Dependency Injection
            var serviceCollection = services
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IEventAggregator, EventAggregator.EventAggregator>()
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

        private static string GetEnvironmentFromAppSettings(System.IO.Stream appSettingsXamarinFormsStream)
        {
            if (appSettingsXamarinFormsStream != null)
            {
                var appSettingsReader = System.Text.Json.JsonDocument.Parse(appSettingsXamarinFormsStream);
                if (appSettingsReader.RootElement.TryGetProperty("environment", out var jsonElement))
                {
                    return jsonElement.GetString();
                }
                appSettingsXamarinFormsStream.Seek(0, System.IO.SeekOrigin.Begin);
            }
            return "Production";
        }
    }
}
