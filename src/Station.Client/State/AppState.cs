using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.State {
	public class AppState : IAppState {

		private readonly IStateStorage _storage;

		public AppState(
			IStateStorage storage
		) {
			IsInitialized = false;
			_storage = storage;

			Authentication = new AuthenticationState();
		}

		public event EventHandler OnStateChanged;

		public bool IsInitialized { get; private set; }

		public IAuthenticationState Authentication { get; private set; }

		public int DisplayWidth { get; set; }

		public int DisplayHeight { get; set; }

		public async Task Initialize() {
			Authentication = await _storage.Get<AuthenticationState>( "State::Authentication" ) ?? new AuthenticationState();
			IsInitialized = true;
			OnStateChanged?.Invoke( this, EventArgs.Empty );
		}

		public async Task Update( IAuthenticationState initial, string accessToken, string refreshToken, DateTime tokensExpireAt ) {
			Authentication = new AuthenticationState( accessToken, refreshToken, tokensExpireAt );
			await _storage.Set( "State::Authentication", Authentication );
			OnStateChanged?.Invoke( this, EventArgs.Empty );
		}
	}
}
