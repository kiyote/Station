using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Station.Client {
	public class Render : IRender {

		private readonly ElementRef _canvas;
		private readonly IJSRuntime _js;

		public Render( ElementRef canvas, IJSRuntime js ) {
			_canvas = canvas;
			_js = js;
		}

		public async Task DrawText( string text, int x, int y ) {
			await _js.InvokeAsync<object>( "render.drawText", _canvas, text, "30px Arial", x, y );
		}

		public async Task Clear() {
			await _js.InvokeAsync<object>( "render.clear", _canvas );
		}

		public async Task Fill() {
			await _js.InvokeAsync<object>( "render.fill", _canvas, "#6495ED" );
		}
	}
}
