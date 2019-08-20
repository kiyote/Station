using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Station.Client.Interop {
	internal class NullRender : IRender {

		public static IRender Instance = new NullRender();

		Task IRender.Clear() {
			throw new NotImplementedException();
		}

		Task IRender.Fill( string colour ) {
			throw new NotImplementedException();
		}

		Task IRender.DrawText( string text, Font font, int x, int y ) {
			throw new NotImplementedException();
		}

		Task IRender.DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy ) {
			throw new NotImplementedException();
		}

		Task IRender.DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh ) {
			throw new NotImplementedException();
		}

		Task IRender.FillRect( string colour, int x, int y, int w, int h ) {
			throw new NotImplementedException();
		}
	}
}
