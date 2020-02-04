using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Station.Client.Interop {
	public class Render : IRender {

		private readonly ElementReference _canvas;
		private readonly IJSRuntime _js;
		private readonly bool _isTransparent;

		public Render(
			ElementReference canvas,
			IJSRuntime js,
			bool isTransparent = false
		) {
			_canvas = canvas;
			_js = js;
			_isTransparent = isTransparent;
		}

		public async Task DrawText( Font font, string colour, string text, int x, int y ) {
			await _js.InvokeVoidAsync( "render.drawText", _canvas, _isTransparent, font.Value, colour, text, x, y );
		}

		public async Task DrawStrokedText( Font font, string colour, string text, int x, int y ) {
			await _js.InvokeVoidAsync( "render.drawStrokedText", _canvas, _isTransparent, font.Value, colour, text, x, y );
		}

		public async Task Clear() {
			await _js.InvokeVoidAsync( "render.clear", _canvas, _isTransparent );
		}

		public async Task Fill( string colour ) {
			await _js.InvokeVoidAsync( "render.fill", _canvas, _isTransparent, colour );
		}

		public async Task FillRect( string colour, int x, int y, int w, int h ) {
			await _js.InvokeVoidAsync( "render.fillRect", _canvas, _isTransparent, colour, x, y, w, h );
		}

		public async Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy ) {
			await _js.InvokeVoidAsync( "render.drawSprite", _canvas, _isTransparent, image, sx, sy, sw, sh, dx, dy );
		}

		public async Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh ) {
			await _js.InvokeVoidAsync( "render.drawSpriteScaled", _canvas, _isTransparent, image, sx, sy, sw, sh, dx, dy, dw, dh );
		}

		public async Task CopyRect( int sx, int sy, int sw, int sh, int dx, int dy ) {
			await _js.InvokeVoidAsync( "render.copyRect", _canvas, _isTransparent, sx, sy, sw, sh, dx, dy );
		}

		public async Task RenderMapBlock( ElementReference image, int x, int y, int columns, int rows, int tileSize, int tileColumns, int tileRows, int[] tiles) {
			await _js.InvokeVoidAsync( "render.renderMapBlock", _canvas, _isTransparent, image, x, y, columns, rows, tileSize, tileColumns, tileRows, tiles );
		}
	}
}
