using System;
using System.Threading.Tasks;

namespace Station.Client.State {
	internal sealed class Dispatch : IDispatch {

		private readonly IAppState _state;

		public Dispatch(
			IAppState state
		) {
			_state = state;
		}

		public async Task UpdateTokens( string idToken, string accessToken, string refreshToken, DateTime tokensExpireAt ) {
			await _state.Update( _state.Authentication, idToken, accessToken, refreshToken, tokensExpireAt );
		}
	}
}
