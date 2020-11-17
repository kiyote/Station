using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Station.Client.Services {
	internal sealed class TokenService : ITokenService {

		private readonly HttpClient _http;
		private readonly IServiceConfig _config;
		private readonly IJsonConverter _json;

		public TokenService(
			HttpClient httpClient,
			IServiceConfig config,
			IJsonConverter json
		) {
			_http = httpClient;
			_config = config;
			_json = json;
		}

		async Task<AuthorizationToken> ITokenService.GetToken( string code ) {
			var content = new FormUrlEncodedContent( new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("client_id", _config.CognitoClientId),
				new KeyValuePair<string, string>("code", code),
				new KeyValuePair<string, string>("redirect_uri", _config.RedirectUrl)
			} );

			HttpResponseMessage response = await _http.PostAsync( _config.TokenUrl, content );
			if( response.IsSuccessStatusCode ) {
				string payload = await response.Content.ReadAsStringAsync();
				AuthorizationToken tokens = _json.Deserialize<AuthorizationToken>( payload );

				return tokens;
			}

			throw new ArgumentException();
		}

		async Task<AuthorizationToken> ITokenService.RefreshToken( string refreshToken ) {
			var content = new FormUrlEncodedContent( new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("client_id", _config.CognitoClientId),
				new KeyValuePair<string, string>("refresh_token", refreshToken)
			} );
			HttpResponseMessage response = await _http.PostAsync( _config.TokenUrl, content );
			if( response.IsSuccessStatusCode ) {
				string payload = await response.Content.ReadAsStringAsync();
				AuthorizationToken tokens = _json.Deserialize<AuthorizationToken>( payload );

				return tokens;
			}

			throw new ArgumentException();
		}
	}
}
