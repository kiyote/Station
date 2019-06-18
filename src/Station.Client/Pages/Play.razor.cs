using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.Pages {
	public class PlayBase : ComponentBase, IDisposable {
		public PlayBase() {
			Width = 800;
			Height = 600;
			JsRuntime = NullJSRuntime.Instance;
			_render = NullRender.Instance;
		}

		protected int Width { get; set; }

		protected int Height { get; set; }

		protected ElementRef? Canvas { get; set; }

		[Inject] protected IJSRuntime JsRuntime { get; set; }

		private IRender _render;

		private int _callbackContext;

		protected override async Task OnAfterRenderAsync() {
			if (Canvas is null) {
				throw new InvalidOperationException();
			}

			_render = new Render( Canvas.Value, JsRuntime );

			_callbackContext = await JsRuntime.InvokeAsync<int>( "anim.start", DotNetObjectRef.Create( this ) );

			Console.WriteLine( "OnAfterRenderAsync" );
		}

		[JSInvokable]
		public Task AnimCallback(int interval) {
			//await _render.Fill();
			//await _render.DrawText( "Hello world!", 15, 30 );
			return Task.CompletedTask;
		}

		[JSInvokable]
		public void SetCallbackContext(int context) {
			_callbackContext = context;
		}

		public void Dispose() {
			((IJSInProcessRuntime)JsRuntime).Invoke<object>( "anim.stop", _callbackContext );
		}
	}
}
