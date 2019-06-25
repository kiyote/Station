using Microsoft.AspNetCore.Http;

namespace Station.Server {
	internal sealed class ContextInformation: IContextInformation {

		private readonly IHttpContextAccessor _httpContextAccessor;

		public ContextInformation( IHttpContextAccessor httpContextAccessor ) {
			_httpContextAccessor = httpContextAccessor;
		}

		string IContextInformation.Username {
			get {
				HttpContext context = _httpContextAccessor.HttpContext;
				return ( context.Items["Username"] as string ) ?? "";
			}
		}
	}
}
