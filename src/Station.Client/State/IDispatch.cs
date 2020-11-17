using System;
using System.Threading.Tasks;

namespace Station.Client.State {
	public interface IDispatch {
		Task UpdateTokens( string idToken, string accessToken, string refreshToken, DateTime tokensExpireAt );
	}
}
