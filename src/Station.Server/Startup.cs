using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Station.Server.Hubs;
using Station.Server.Middleware;

namespace Station.Server {
	public class Startup {
		public Startup( IConfiguration configuration ) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices( IServiceCollection services ) {
			services
				.AddConnections()
				.AddSignalR( o => o.KeepAliveInterval = TimeSpan.FromSeconds( 5 ) )
				.AddJsonProtocol();

			services.AddAuthorization( options => {
				options.AddPolicy( JwtBearerDefaults.AuthenticationScheme, policy => {
					policy.AddAuthenticationSchemes( JwtBearerDefaults.AuthenticationScheme );
					policy.RequireClaim( ClaimTypes.NameIdentifier );
				} );
				options.DefaultPolicy = options.GetPolicy( JwtBearerDefaults.AuthenticationScheme );
			} );

			services
				.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
				.AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, SetJwtBearerOptions );

			// TODO: Determine if this is still needed for SignalR
			services.AddCors( options => options.AddPolicy( "CorsPolicy",
				 builder => {
					 builder.AllowAnyMethod()
						 .AllowAnyHeader()
						 .AllowAnyOrigin();
				 } ) );

			services
				.AddMvc()
				.SetCompatibilityVersion( Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0 )
				.AddNewtonsoftJson( options => {
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
					options.SerializerSettings.DateParseHandling = DateParseHandling.None;
					options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
				} );
			services.AddResponseCompression( opts => {
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" } );
			} );

			services.AddHttpContextAccessor();
			services.AddSingleton<IContextInformation, ContextInformation>();
		}

		public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {

			if( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
				app.UseBlazorDebugging();
			}
			app.UseCors( "CorsPolicy" );
			app.UseClientSideBlazorFiles<Client.Program>();
			app.UseRouting();
			app.UseResponseCompression();
			app.UseAuthorization();
			app.UseAuthentication();
			app.UseIdentificationMiddleware();

			app
				.UseEndpoints( endpoints => {
					endpoints.MapHub<SignalHub>( SignalHub.Url );
					endpoints.MapDefaultControllerRoute();
					endpoints.MapFallbackToClientSideBlazor<Client.Program>( "index.html" );
				} );
		}

		private void SetJwtBearerOptions( JwtBearerOptions options ) {
			TokenValidationOptions tokenValidationOptions = Configuration.GetSection( "TokenValidation" ).Get<TokenValidationOptions>();
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(
				new RSAParameters() {
					Modulus = Base64UrlEncoder.DecodeBytes( tokenValidationOptions.Modulus ),
					Exponent = Base64UrlEncoder.DecodeBytes( tokenValidationOptions.Expo )
				} );
			var key = new RsaSecurityKey( rsa );

			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters {
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = key,
				ValidIssuer = tokenValidationOptions.Issuer,
				ValidateIssuer = true,
				ValidateLifetime = true,
				ValidateAudience = false,
				ClockSkew = TimeSpan.FromMinutes( 0 )
			};

			options.Events = new JwtBearerEvents {
				OnMessageReceived = context => {
					StringValues accessToken = context.Request.Query["access_token"];

					// If there is an access token supplied in the url, then
					// we check to see if we're actually trying to service
					// a SignalR request, and if so, we tuck the token in to
					// the context so the request is property authenticated.
					if( !string.IsNullOrWhiteSpace( accessToken )
						&& ( context.HttpContext.WebSockets.IsWebSocketRequest
							|| context.HttpContext.Request.Path.StartsWithSegments( SignalHub.Url )
							|| context.Request.Headers["Accept"] == "text/event-stream" ) ) {
						context.Token = context.Request.Query["access_token"];
					}
					return Task.CompletedTask;
				}
			};
		}
	}
}
