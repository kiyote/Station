using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Station.Client.Services;
using Station.Client.State;
using Station.Client.Interop;

namespace Station.Client {
	public class Startup {
		public void ConfigureServices( IServiceCollection services ) {

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
		}

		public void Configure( IComponentsApplicationBuilder app ) {
			app.AddComponent<App>( "app" );
		}
	}
}
