/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Station.Client.Services;
using Station.Client.State;

namespace Station.Client {
	public class Startup {
		public void ConfigureServices( IServiceCollection services ) {
			services.AddSingleton<IJSRuntimeProvider, JSRuntimeProvider>();
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton<IJsonConverter, JsonConverter>();
			services.AddSingleton<IServiceConfig, Config>();
			services.AddSingleton<ITokenService, TokenService>();
			services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
			services.AddSingleton<ISignalService, SignalService>();
			services.AddSingleton<IStateStorage, StateStorage>();
			services.AddSingleton<IAppState, AppState>();
			services.AddSingleton<IUserApiService, UserApiService>();
		}

		public void Configure( IComponentsApplicationBuilder app ) {
			app.AddComponent<App>( "app" );
		}
	}
}
