using System;
using System.Threading.Tasks;
using Station.Client.State;

namespace Station.Client.Services {
	internal sealed class AccessTokenProvider : IAccessTokenProvider {

		private readonly IAppState _state;
		private readonly ITokenService _tokenService;

		public AccessTokenProvider(
			IAppState state,
			ITokenService tokenService
		) {
			_state = state;
			_tokenService = tokenService;
		}

		async Task<string> IAccessTokenProvider.GetJwtToken() {

			if( _state.Authentication.IdToken == default ) {
				throw new InvalidOperationException();
			}

			if( _state.Authentication.TokensExpireAt < DateTimeOffset.Now ) {
				AuthorizationToken tokens = await _tokenService.RefreshToken( _state.Authentication.AccessToken );
				if( tokens != default ) {
					await _state.Update( _state.Authentication, tokens.id_token, tokens.access_token, tokens.refresh_token, DateTime.UtcNow.AddSeconds( tokens.expires_in ) );
				}
			}
			return _state.Authentication.IdToken;
		}
	}
}
