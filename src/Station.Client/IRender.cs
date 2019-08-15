using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Station.Client {
	public interface IRender {

		Task Clear();

		Task Fill( string colour );

		Task DrawText( string text, Font font, int x, int y );

		Task DrawSprite( ElementReference image, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh );
	}
}
