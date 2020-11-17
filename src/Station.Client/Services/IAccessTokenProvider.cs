using System;
using System.Threading.Tasks;

namespace Station.Client.Services {
	public interface IAccessTokenProvider {

		Task<string> GetJwtToken();
	}
}
