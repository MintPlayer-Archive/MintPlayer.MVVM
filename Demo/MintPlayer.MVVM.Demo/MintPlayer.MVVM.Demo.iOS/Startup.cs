using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MintPlayer.MVVM.Demo.iOS
{
    public class Startup : Demo.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}