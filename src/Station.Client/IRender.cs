using System.Threading.Tasks;

namespace Station.Client {
	public interface IRender {

		Task Clear();

		Task Fill();

		Task DrawText( string text, int x, int y );
	}
}
