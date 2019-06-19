using System;
using System.Threading.Tasks;

namespace Station.Client.State {
	public class NullState : IAppState {

		public static IAppState Instance = new NullState();

		int IAppState.DisplayWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		int IAppState.DisplayHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		IAuthenticationState IAppState.Authentication => throw new NotImplementedException();

		bool IAppState.IsInitialized => throw new NotImplementedException();

		event EventHandler IAppState.OnStateChanged {
			add {
				throw new NotImplementedException();
			}

			remove {
				throw new NotImplementedException();
			}
		}

		Task IAppState.Initialize() {
			throw new NotImplementedException();
		}

		Task IAppState.Update( IAuthenticationState initial, string accessToken, string refreshToken, DateTime tokensExpireAt ) {
			throw new NotImplementedException();
		}
	}
}
