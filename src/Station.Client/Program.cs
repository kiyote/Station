using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Station.Client.Interop;
using Station.Client.Services;
using Station.Client.Services.Map;
using Station.Client.State;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Station.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			ConfigureServices( builder.Services );
			await builder.Build().RunAsync();
        }

		private static void ConfigureServices( IServiceCollection services ) {
			services.AddSingleton<IJSRuntimeProvider, JSRuntimeProvider>();
			services.AddSingleton<IJsonConverter, JsonConverter>();
			services.AddScoped<IAuthenticationState, AuthenticationState>();
			services.AddScoped<IDispatch, Dispatch>();
			services.AddScoped<IStateStorage, StateStorage>();
			services.AddScoped<IAppState, AppState>();
			services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();
			services.AddScoped<IConfig, Config>();
			services.AddScoped<IServiceConfig, Config>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<ISignalService, SignalService>();
			services.AddScoped<IAnim, Anim>();
			services.AddScoped<IMapService, MapService>();
			services.AddScoped<IMapRenderer, MapRenderer>();
		}
	}
}
