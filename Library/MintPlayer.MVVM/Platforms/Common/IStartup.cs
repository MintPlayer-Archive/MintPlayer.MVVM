using Microsoft.Extensions.DependencyInjection;

namespace MintPlayer.MVVM.Platforms.Common
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
