using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Station.Client.State;

namespace Station.Client.Pages {
	public class IndexBase: ComponentBase {

		public IndexBase() {
			State = NullState.Instance;
			JSRuntime = NullJSRuntime.Instance;
		}

		[Inject] protected IAppState State { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		[Inject] protected IConfig Config { get; set; }

		public string LogInUrl {
			get {
				return $"{Config.LogInUrl}&redirect_uri={Config.RedirectUrl}";
			}
		}

		protected override async Task OnAfterRenderAsync() {
			if ( JSRuntime == null || State == null ) {
				return;
			}

			int width = await JSRuntime.InvokeAsync<int>( "app.getWidth" );
			int height = await JSRuntime.InvokeAsync<int>( "app.getHeight" );
			State.DisplayWidth = width;
			State.DisplayHeight = height;
		}
	}
}
