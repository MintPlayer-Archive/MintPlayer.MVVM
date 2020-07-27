using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MintPlayer.MVVM.Platforms.Common
{
    public static class MintPlayerMvvmExtensions
    {
        public static IServiceCollection AddMintPlayerMvvm(this IServiceCollection services)
        {
            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .Build();

            // Setup Dependency Injection
            var serviceCollection = services
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<INavigationService, NavigationService>();

            return serviceCollection;
        }
    }
}
