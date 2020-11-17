using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Interop;
using Station.Shared;

namespace Station.Client.Services.Map {
	public class MapRenderer : IMapRenderer {

		private readonly IMapService _map;

		public MapRenderer(
			IMapService mapService
		) {
			_map = mapService;
		}

		public async Task RenderTerrain(
			IRender terrainCanvas,
			ElementReference terrainImage
		) {
			await terrainCanvas.Clear();
			for (int x = 0; x < 10; x ++) {
				for (int y = 0; y < 10; y++) {
					int[] terrain = _map.GetTerrainChunk( x, y );

					if (terrain != Array.Empty<int>()) {
						await terrainCanvas.RenderMapBlock(
							terrainImage,
							( x * TerrainChunk.ChunkSize * 32 ),
							( y * TerrainChunk.ChunkSize * 32 ),
							TerrainChunk.ChunkSize,
							TerrainChunk.ChunkSize,
							32,
							10,
							10,
							terrain );
					}
				}
			}
		}
	}
}
