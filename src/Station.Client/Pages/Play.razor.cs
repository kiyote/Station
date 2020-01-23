using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Station.Client.Services;
using Station.Client.State;
using Station.Client.Interop;

namespace Station.Client.Pages {
	public class PlayBase : ComponentBase, IAnimCallback, IDisposable {

		private int _width;
		private int _height;

		[Inject] protected IJSRuntime JsRuntime { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected ISignalService Signal { get; set; }

		[Inject] protected IAnim Anim { get; set; }

		[CascadingParameter] protected AssetManagerBase AssetManager { get; set; }

		protected int Width {
			get {
				return _width;
			}
			set {
				_width = value;
			}
		}

		protected int Height {
			get {
				return _height;
			}
			set {
				_height = value;
			}
		}

		protected ElementReference? Canvas { get; set; }

		private IRender _render;

		private readonly Font _font;

		private int _frameCount;
		private int _frameCounter;
		private float _elapsedTime;

		public PlayBase() {
			Width = 800;
			Height = 600;
			JsRuntime = NullJSRuntime.Instance;
			_render = NullRender.Instance;
			State = NullState.Instance;

			_font = new Font( "Arial", 16 );

			_elapsedTime = 0.0f;
			_frameCount = 0;
			_frameCounter = 0;
		}

		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) {
			if (disposing) {
				Anim.Stop();
			}
		}

		protected override async Task OnInitializedAsync() {
			if( State is null ) {
				return;
			}

			ResizeCanvas( State, out _width, out _height );

			await Signal.Connect();
		}

		protected override async Task OnAfterRenderAsync( bool firstRender ) {
			if( Canvas is null ) {
				throw new InvalidOperationException();
			}

			_render = new Render( Canvas.Value, JsRuntime );

			await Anim.Start( this );
		}

		async Task IAnimCallback.RenderFrame( float interval ) {
			await _render.Fill( Colour.CornflowerBlue );
			for( int y = 0; y < 1; y++ ) {
				for( int x = 0; x < 1; x++ ) {
					await _render.DrawSprite( AssetManager.Terrain.Value, 0, 0, 32, 32, ( x * 32 ), ( y * 32 ) );
				}
			}
			await _render.DrawStrokedText( _font, Colour.White, $"FPS: {_frameCount}", 50, 30 );

			_frameCounter++;
			_elapsedTime += interval;
			if( _elapsedTime >= 1000.0f ) {
				_frameCount = _frameCounter;
				_frameCounter = 0;
				_elapsedTime -= 1000.0f;
			}
		}

		private static void ResizeCanvas( IAppState state, out int width, out int height ) {
			if( state.DisplayWidth < state.DisplayHeight ) {
				width = state.DisplayWidth - ( state.DisplayWidth % 100 );
				height = (int)( (float)width * 9.0f / 16.0f );
			} else {
				height = state.DisplayHeight - ( state.DisplayHeight % 100 );
				width = (int)( (float)height * 16.0f / 9.0f );
			}
		}

	}
}
