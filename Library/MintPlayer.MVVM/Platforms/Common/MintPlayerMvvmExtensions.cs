using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MintPlayer.MVVM.Platforms.Common
{
    public static class MintPlayerMvvmExtensions
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

            var appSettingsXamarinFormsStream = tstartup.BaseType.Assembly.GetManifestResourceStream($"{tstartup.BaseType.Assembly.GetName().Name}.appsettings.json");
            var appSettingsPlatformStream = tstartup.Assembly.GetManifestResourceStream($"{tstartup.Assembly.GetName().Name}.appsettings.json");

            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonStream(appSettingsXamarinFormsStream)
                .AddJsonStream(appSettingsPlatformStream)
                .Build();

            // Setup Dependency Injection
            var serviceCollection = services
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<INavigationService, NavigationService>();

            // Create an instance of the startup class
            var startup = (TStartup)tstartup.GetConstructor(new[] { typeof(IConfiguration) }).Invoke(new object[] { configuration });
            startup.ConfigureServices(serviceCollection);

            return serviceCollection;
        }
    }
}
