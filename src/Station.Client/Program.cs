using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Station.Client.Interop;
using Station.Client.Services;
using Station.Client.Services.Map;
using Station.Client.State;

namespace Station.Client {
	public class Program {

		public static async Task Main( string[] args ) {
			var builder = WebAssemblyHostBuilder.CreateDefault( args );
			ConfigureServices( builder.Services );

			builder.RootComponents.Add<App>( "app" );
			WebAssemblyHost host = builder.Build();

			// Access registered services here

			await host.RunAsync();
		}

		private static void ConfigureServices( IServiceCollection services ) {
			services.AddSingleton<IJSRuntimeProvider, JSRuntimeProvider>();
			services.AddSingleton<IJsonConverter, JsonConverter>();
			services.AddSingleton<IAuthenticationState, AuthenticationState>();
			services.AddSingleton<IDispatch, Dispatch>();
			services.AddSingleton<IStateStorage, StateStorage>();
			services.AddSingleton<IAppState, AppState>();
			services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton<IServiceConfig, Config>();
			services.AddSingleton<ITokenService, TokenService>();
			services.AddSingleton<ISignalService, SignalService>();
			services.AddSingleton<IAnim, Anim>();
			services.AddSingleton<IMapService, MapService>();
			services.AddSingleton<IMapRenderer, MapRenderer>();
		}
	}
}
