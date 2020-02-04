/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
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
