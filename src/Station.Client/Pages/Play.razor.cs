using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Station.Client.Services;
using Station.Client.State;

namespace Station.Client.Pages {
	public class PlayBase : ComponentBase, IDisposable {
		public PlayBase() {
			Width = 800;
			Height = 600;
			JsRuntime = NullJSRuntime.Instance;
			_render = NullRender.Instance;
			State = NullState.Instance;
		}

		[Inject] protected IJSRuntime JsRuntime { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected ISignalService Signal { get; set; }

		protected int Width { get; set; }

		protected int Height { get; set; }

		protected ElementRef? Canvas { get; set; }

		private IRender _render;

		private int _callbackContext;

		protected override async Task OnInitAsync() {
			if (State == null) {
				return;
			}

			if (State.DisplayWidth < State.DisplayHeight) {
				Width = State.DisplayWidth - ( State.DisplayWidth % 100 );
				Height = (int)((float)Width * 9.0f / 16.0f);
			} else {
				Height = State.DisplayHeight - ( State.DisplayHeight % 100 );
				Width = (int)( (float)Height * 16.0f / 9.0f );
			}

			await Signal.Connect();
		}

		protected override async Task OnAfterRenderAsync() {
			if (Canvas is null) {
				throw new InvalidOperationException();
			}

			_render = new Render( Canvas.Value, JsRuntime );

			_callbackContext = await JsRuntime.InvokeAsync<int>( "anim.start", DotNetObjectRef.Create( this ) );

			Console.WriteLine( "OnAfterRenderAsync" );
		}

		[JSInvokable]
		public async Task AnimCallback(int interval) {
			await _render.Fill();
			await _render.DrawText( "Hello world!", 15, 30 );
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
