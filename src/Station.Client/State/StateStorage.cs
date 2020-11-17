using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Station.Client.Services;

namespace Station.Client.State {
	internal sealed class StateStorage : IStateStorage {

		private readonly IJSRuntimeProvider _jsProvider;
		private readonly IJsonConverter _json;

		public StateStorage(
			IJSRuntimeProvider jsRuntimeProvider,
			IJsonConverter jsonConverter
		) {
			_jsProvider = jsRuntimeProvider;
			_json = jsonConverter;
		}

		public async Task<DateTime> GetAsDateTime( string name ) {
			IJSRuntime js = _jsProvider.Get();
			string value = await js.InvokeAsync<string>( "appState.getItem", name );

			if( string.IsNullOrWhiteSpace( value ) ) {
				return DateTime.MinValue.ToUniversalTime();
			}

			return DateTime.Parse( value ).ToUniversalTime();
		}

		public async Task<string> GetAsString( string name ) {
			IJSRuntime js = _jsProvider.Get();
			return await js.InvokeAsync<string>( "appState.getItem", name );
		}

		public async Task<int> GetAsInt( string name ) {
			IJSRuntime js = _jsProvider.Get();
			string value = await js.InvokeAsync<string>( "appState.getItem", name );
			return int.Parse( value );
		}

		public async Task<T> Get<T>( string name ) {
			IJSRuntime js = _jsProvider.Get();
			string value = await js.InvokeAsync<string>( "appState.getItem", name );
			if( string.IsNullOrWhiteSpace( value ) ) {
				return default!;
			}
			return _json.Deserialize<T>( value );
		}

		public async Task Set( string name, string value ) {
			IJSRuntime js = _jsProvider.Get();
			await js.InvokeAsync<string>( "appState.setItem", name, value );
		}

		public async Task Set( string name, int value ) {
			IJSRuntime js = _jsProvider.Get();
			await js.InvokeAsync<string>( "appState.setItem", name, value );
		}

		public async Task Set( string name, DateTime value ) {
			IJSRuntime js = _jsProvider.Get();
			await js.InvokeAsync<string>( "appState.setItem", name, value.ToString( "o" ) );
		}

		public async Task Set<T>( string name, T value ) {
			string json = _json.Serialize( value );
			IJSRuntime js = _jsProvider.Get();
			await js.InvokeAsync<string>( "appState.setItem", name, json );
		}
	}
}
