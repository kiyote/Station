using System;
using System.Runtime.Serialization;

namespace Station.Client.State {
	public interface IAuthenticationState {

		string IdToken { get; }

		string AccessToken { get; }

		string RefreshToken { get; }

		DateTime TokensExpireAt { get; }

		[IgnoreDataMember]
		bool IsAuthenticated { get; }
	}
}
