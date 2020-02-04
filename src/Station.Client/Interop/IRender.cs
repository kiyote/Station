using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Station.Client.Interop {
	public interface IRender {

		Task Clear();

		Task Fill( string colour );

		Task DrawText( Font font, string colour, string text, int x, int y );

		Task DrawStrokedText( Font font, string colour, string text, int x, int y );

		Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy );

		Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh );

		Task FillRect( string colour, int x, int y, int w, int h );

		Task CopyRect( int sx, int sy, int sw, int sh, int dx, int dy );

		Task RenderMapBlock( ElementReference image, int x, int y, int columns, int rows, int tileSize, int tileColumns, int tileRows, int[] tiles );
	}
}
