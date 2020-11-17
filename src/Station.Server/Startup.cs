using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Station.Server.Hubs;
using Station.Server.Middleware;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Station.Server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
			services
				.AddConnections()
				.AddSignalR( o => o.KeepAliveInterval = TimeSpan.FromSeconds( 5 ) );

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

			services.AddCors( options => options.AddPolicy( "CorsPolicy",
				 builder => {
					 builder.AllowAnyMethod()
						 .AllowAnyHeader()
						 .AllowAnyOrigin();
				 } ) );

			services.AddControllers();
            services.AddRazorPages();

			services.AddHttpContextAccessor();
			services.AddSingleton<IContextInformation, ContextInformation>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

			app.UseCors( "CorsPolicy" );
			app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
			app.UseIdentificationMiddleware();
			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints => {
				endpoints.MapHub<SignalHub>( SignalHub.Url );
				endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
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
					string accessToken = context.Request.Query["access_token"].FirstOrDefault();

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
