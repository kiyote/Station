/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Station.Server.Middleware {
	public sealed class IdentificationMiddleware {

		private readonly RequestDelegate _next;

		public IdentificationMiddleware(
			RequestDelegate next
		) {
			_next = next;
		}

		public async Task InvokeAsync( HttpContext httpContext ) {
			var principal = httpContext.User?.Identities?.FirstOrDefault();

			if( principal?.Claims?.Any() ?? false ) {
				var userIdValue = principal.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier )?.Value;
				var username = principal.Claims.FirstOrDefault( c => c.Type == "name" )?.Value;
				httpContext.Items["Username"] = username;
			}

			await _next( httpContext );
		}
	}

	public static class IdentificationMiddlewareExtensions {
		public static IApplicationBuilder UseIdentificationMiddleware( this IApplicationBuilder builder ) {
			return builder.UseMiddleware<IdentificationMiddleware>();
		}
	}
}
