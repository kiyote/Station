using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Station.Client.Interop {
	public class Render : IRender {

		private readonly ElementReference _canvas;
		private readonly IJSRuntime _js;

		public Render( ElementReference canvas, IJSRuntime js ) {
			_canvas = canvas;
			_js = js;
		}

		public async Task DrawText( string text, Font font, int x, int y ) {
			await _js.InvokeAsync<object>( "render.drawText", _canvas, text, font.Value, x, y );
		}

		public async Task Clear() {
			await _js.InvokeAsync<object>( "render.clear", _canvas );
		}

		public async Task Fill( string colour ) {
			await _js.InvokeAsync<object>( "render.fill", _canvas, colour );
		}

		public async Task FillRect( string colour, int x, int y, int w, int h ) {
			await _js.InvokeAsync<object>( "render.fill", _canvas, colour, x, y, w, h );
		}

		public async Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh ) {
			await _js.InvokeAsync<object>( "render.drawSprite", _canvas, image, sx, sy, sw, sh, dx, dy, dw, dh );
		}
	}
}
