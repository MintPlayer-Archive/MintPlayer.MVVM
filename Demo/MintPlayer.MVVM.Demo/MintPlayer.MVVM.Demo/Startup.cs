using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.MVVM.Demo.Services;
using MintPlayer.MVVM.Platforms.Common;
using System.Net.Http;

namespace MintPlayer.MVVM.Demo
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<HttpClient>((provider) => { var client = new HttpClient(); client.DefaultRequestHeaders.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")); return client; })
                .AddScoped<IArtistService, ArtistService>()
                .AddSingleton(Configuration);
        }
    }
}
