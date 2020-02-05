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
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Station.Client.Services {
	internal sealed class SignalService : ISignalService {

		private readonly HubConnection _connection;
		private bool _connected;

		public SignalService(
			IAccessTokenProvider accessTokenProvider,
			NavigationManager navigationManager
		) {
			_connected = false;
			HubConnectionBuilder factory = new HubConnectionBuilder();

			factory
				.WithUrl( navigationManager.ToAbsoluteUri( "/signalhub" ), opt => {
					opt.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
					opt.AccessTokenProvider = accessTokenProvider.GetJwtToken;
					opt.DefaultTransferFormat = TransferFormat.Binary;
				} )
				.WithAutomaticReconnect();

			_connection = factory.Build();
		}

		IDisposable ISignalService.Register<T>( string name, Action<T> handler ) {
			return _connection.On( name, handler );
		}

		async Task ISignalService.Connect() {
			if( !_connected ) {
				_connected = true;
				await _connection.StartAsync();
			}
		}

		async Task ISignalService.Invoke<T>( string name, T payload ) {
			await _connection.InvokeAsync( name, payload );
		}

		async Task ISignalService.Invoke<S, T>( string name, S arg1, T arg2 ) {
			await _connection.InvokeAsync( name, arg1, arg2 );
		}
	}
}
