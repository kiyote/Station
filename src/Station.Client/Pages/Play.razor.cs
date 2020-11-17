using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Station.Client.Services;
using Station.Client.State;
using Station.Client.Interop;
using Station.Shared;

namespace Station.Client.Pages {
	public partial class Play : IAnimCallback, IDisposable {

		protected ElementReference? Canvas { get; set; }
		protected ElementReference? TerrainCanvas { get; set; }

		private int _width;
		private int _height;
		private IRender _render;
		private IRender _terrainRender;
		private readonly Font _font;
		private int _frameCount;
		private int _frameCounter;
		private float _elapsedTime;
		private IDisposable _sendFromServerHandle;
		private bool _terrainDirty;

		[Inject] protected IJSRuntime JsRuntime { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected ISignalService Signal { get; set; }

		[Inject] protected IMapRenderer MapRenderer { get; set; }

		[Inject] protected IAnim Anim { get; set; }

		[Inject] protected IMapService Map { get; set; }

		[CascadingParameter] protected AssetManager AssetManager { get; set; }

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

		public Play() {
			Width = 800;
			Height = 600;
			JsRuntime = NullJSRuntime.Instance;
			_render = NullRender.Instance;
			_terrainRender = NullRender.Instance;
			State = NullState.Instance;

			_font = new Font( "Arial", 16 );

			_elapsedTime = 0.0f;
			_frameCount = 0;
			_frameCounter = 0;
			_terrainDirty = true;
		}

		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) {
			if( disposing ) {
				Anim.Stop();
				_sendFromServerHandle.Dispose();
			}
		}

		protected override async Task OnInitializedAsync() {
			if( State is null ) {
				return;
			}

			ResizeCanvas( State, out _width, out _height );

			Map.Register( TerrainChanged );
			_sendFromServerHandle = Signal.Register<string>( "Send", SendFromServer );
			await Signal.Connect();
		}

		protected override async Task OnAfterRenderAsync( bool firstRender ) {
			if( Canvas is null ) {
				throw new InvalidOperationException();
			}

			if( firstRender ) {
				_render = new Render( Canvas.Value, JsRuntime, true );
				_terrainRender = new Render( TerrainCanvas.Value, JsRuntime, false );

				await Anim.Start( this );

				await Map.SetVisibleArea( new Rect( 0, 0, _width / 32, _height / 32 ) );
			}
		}

		async Task IAnimCallback.RenderFrame( float interval ) {

			await _render.Clear();
			await _render.DrawStrokedText( _font, Colour.White, $"FPS: {_frameCount}", 50, 30 );

			if( _terrainDirty ) {
				await MapRenderer.RenderTerrain( _terrainRender, AssetManager.Terrain.Value );
				_terrainDirty = false;
			}

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

		private void SendFromServer( string payload ) {
			Console.WriteLine( payload );
		}

		private void TerrainChanged() {
			_terrainDirty = true;
		}
	}
}
