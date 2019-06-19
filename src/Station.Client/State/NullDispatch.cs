using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.State {
	public class NullDispatch : IDispatch {

		public static IDispatch Instance = new NullDispatch();

		Task IDispatch.UpdateTokens( string accessToken, string refreshToken, DateTime tokensExpireAt ) {
			throw new NotImplementedException();
		}
	}
}
