using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.Services {
	public class NullTokenService : ITokenService {

		public static ITokenService Instance = new NullTokenService();

		Task<AuthorizationToken> ITokenService.GetToken( string code ) {
			throw new NotImplementedException();
		}

		Task<AuthorizationToken> ITokenService.RefreshToken( string refreshToken ) {
			throw new NotImplementedException();
		}
	}
}
