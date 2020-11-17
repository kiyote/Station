using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Services;
using Station.Client.State;

#nullable enable

namespace Station.Client.Pages.Auth {
	public partial class ValidatePage : IDisposable {

		public static string Url = "/auth/validate";

		public ValidatePage() {
			Messages = new List<string>();
			UriHelper = NullUriHelper.Instance;
			State = NullState.Instance;
			Dispatch = NullDispatch.Instance;
			TokenService = NullTokenService.Instance;
		}

		[Inject] protected NavigationManager UriHelper { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected IDispatch Dispatch { get; set; }

		[Inject] protected ITokenService TokenService { get; set; }

		protected List<string> Messages { get; set; }

		protected int Progress { get; set; }

		protected override async Task OnInitializedAsync() {
			State.OnStateChanged += AppStateHasChanged;
			await State.Initialize();

			string code = UriHelper.GetParameter( "code" );
			Messages.Add( "...retrieving token..." );
			AuthorizationToken tokens = await TokenService.GetToken( code );
			if( tokens == default ) {
				//TODO: Do something here
				throw new InvalidOperationException();
			}
			await Dispatch.UpdateTokens( tokens.id_token, tokens.access_token, tokens.refresh_token, DateTime.UtcNow.AddSeconds( tokens.expires_in ) );

			Update( "...recording login...", 50 );
			//await Dispatch.RecordLogin();

			Update( "...loading user information...", 75 );
			//await Dispatch.LoadUserInformation();

			UriHelper.NavigateTo( "/" );
		}

		protected virtual void Dispose( bool disposing ) {
			if (disposing) {
				State.OnStateChanged -= AppStateHasChanged;
			}
		}

		private void AppStateHasChanged( object sender, EventArgs e ) {
			StateHasChanged();
		}

		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Update( string message, int progress ) {
			Messages.Add( message );
			Progress = progress;
			StateHasChanged();
		}
	}
}
