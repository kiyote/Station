using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Station.Client.Services;
using Station.Client.State;

namespace Station.Client.Pages {
	public class PlayBase : ComponentBase, IDisposable {

		[Inject] protected IJSRuntime JsRuntime { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected ISignalService Signal { get; set; }

		[CascadingParameter] protected AssetManagerBase AssetManager { get; set; }

		protected int Width { get; set; }

		protected int Height { get; set; }

		protected ElementReference? Canvas { get; set; }

		private IRender _render;

		private int _callbackContext;

		private readonly Font _font;

		public PlayBase() {
			Width = 800;
			Height = 600;
			JsRuntime = NullJSRuntime.Instance;
			_render = NullRender.Instance;
			State = NullState.Instance;

			_font = new Font( "Arial", 16 );
		}

		public void Dispose() {
			if( _callbackContext != -1 ) {
				( (IJSInProcessRuntime)JsRuntime ).Invoke<object>( "anim.stop", _callbackContext );
			}
		}

		[JSInvokable]
		public async Task AnimCallback( int interval ) {
			await _render.Fill( Colour.CornflowerBlue );
			await _render.DrawText( "Hello world!", _font, 15, 30 );
			await _render.DrawSprite( AssetManager.Terrain.Value, 50, 20, 40, 40, 50, 50, 40, 40 );
		}

		[JSInvokable]
		public void SetCallbackContext( int context ) {
			_callbackContext = context;
		}

		protected override async Task OnInitializedAsync() {
			if (State == null) {
				return;
			}

			_callbackContext = -1;

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
		}
	}
}
