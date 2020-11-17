using System;
using System.Threading.Tasks;
using Station.Shared;

namespace Station.Client.Services {
	public interface IMapService {

		void Register( Action terrainChanged );

		Task SetVisibleArea( Rect area );

		int GetTerrain( int column, int row );

		int[] GetTerrainChunk( int chunkColumn, int chunkRow );
	}
}
