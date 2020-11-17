using System.Threading.Tasks;

namespace Station.Client.Services {
	public interface ITokenService {
		Task<AuthorizationToken> GetToken( string code );

		Task<AuthorizationToken> RefreshToken( string refreshToken );
	}
}
