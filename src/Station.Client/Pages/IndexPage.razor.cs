using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Services;

namespace Station.Client.Pages {
	public class IndexPageBase: ComponentBase, IDisposable {

		public const string Url = "/";

		[Inject] protected IUriHelper UriHelper { get; set; }

		[Inject] protected IConfig Config { get; set; }

		[Inject] protected ISignalService SignalService { get; set; }

		[Inject] protected IJsonConverter Json { get; set; }

		protected List<string> Messages { get; set; }

		private IDisposable _chatHandler;

		public IndexPageBase(): base() {
			Messages = new List<string>();
		}

		protected override async Task OnInitAsync() {
			_chatHandler = SignalService.Register<string>( "Send", HandleUserChat );
			await SignalService.Connect();
		}

		public void Dispose() {
			_chatHandler.Dispose();
		}

		public void GoToLogin() {
			UriHelper.NavigateTo( $"{Config.LogInUrl}&redirect_uri={Config.RedirectUrl}" );
		}

		public async Task SendMessage() {
			await SignalService.Invoke( "Send", "Hello, World!" );
		}

		private void HandleUserChat( string payload ) {
			Messages.Add( payload );
			this.StateHasChanged();
		}

	}
}
