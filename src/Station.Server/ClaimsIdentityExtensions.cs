using System.Linq;
using System.Security.Claims;

namespace Station.Server {
	public static class ClaimsIdentityExtensions {

		public static string GetUsername( this ClaimsIdentity principal ) {
			if( principal?.Claims?.Any() ?? false ) {
				string userIdValue = principal.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier )?.Value;
				string username = principal.Claims.FirstOrDefault( c => c.Type == "name" )?.Value;
				return username;
			}

			return default;
		}
	}
}
