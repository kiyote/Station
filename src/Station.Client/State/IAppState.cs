using System;
using System.Threading.Tasks;

namespace Station.Client.State {
	public interface IAppState {

		event EventHandler OnStateChanged;

		IAuthenticationState Authentication { get; }

		public bool IsInitialized { get; }

		public int DisplayWidth { get; set; }

		public int DisplayHeight { get; set; }

		Task Initialize();

		Task Update( IAuthenticationState initial, string accessToken, string refreshToken, DateTime tokensExpireAt );
	}
}
