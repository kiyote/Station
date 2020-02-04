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

		Task IRender.DrawText( Font font, string colour, string text, int x, int y ) {
			throw new NotImplementedException();
		}

		Task IRender.DrawStrokedText( Font font, string colour, string text, int x, int y ) {
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

		Task IRender.CopyRect( int sx, int sy, int sw, int sh, int x, int y ) {
			throw new NotImplementedException();
		}

		Task IRender.RenderMapBlock( ElementReference image, int x, int y, int columns, int rows, int tileSize, int tileColumns, int tileRows, int[] tiles ) {
			throw new NotImplementedException();
		}
	}
}
