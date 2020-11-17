using System;
using System.Runtime.Serialization;

namespace Station.Client.State {
	internal sealed class AuthenticationState : IAuthenticationState {

		public AuthenticationState() {
			TokensExpireAt = DateTime.MinValue.ToUniversalTime();
		}

		public AuthenticationState(
			string idToken,
			string accessToken,
			string refreshToken,
			DateTime tokensExpireAt
		) {
			IdToken = idToken;
			AccessToken = accessToken;
			RefreshToken = refreshToken;
			TokensExpireAt = tokensExpireAt;
		}

		public string IdToken { get; }

		public string AccessToken { get; }

		public string RefreshToken { get; }

		public DateTime TokensExpireAt { get; }

		[IgnoreDataMember]
		public bool IsAuthenticated {
			get {
				return ( TokensExpireAt >= DateTime.UtcNow );
			}
		}
	}
}
