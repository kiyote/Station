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
using Station.Shared;

namespace Station.Client.Services.Map {
	internal sealed class MapService: IMapService {

		private readonly ISignalService _signal;
		private readonly TerrainCache _cache;
		private Rect _visibleArea;
		private Action _terrainChanged;

		public MapService(
			ISignalService signalService
		) {
			_signal = signalService;
			_cache = new TerrainCache();
			_signal.Register<TerrainChunk>( "TerrainChunk", TerrainChunkCallback );
		}

		public void Register( Action terrainChanged ) {
			_terrainChanged = terrainChanged;
		}

		public async Task SetVisibleArea( Rect area ) {
			_visibleArea = area;
			int startChunkColumn = area.X / TerrainChunk.ChunkSize;
			int endChunkColumn = ( area.X + area.W ) / TerrainChunk.ChunkSize;
			int startChunkRow = area.Y / TerrainChunk.ChunkSize;
			int endChunkRow = ( area.Y + area.H ) / TerrainChunk.ChunkSize;

			for( int c = startChunkColumn; c <= endChunkColumn ; c++ ) {
				for (int r = startChunkRow; r <= endChunkRow; r++ ) {

					if (!_cache.Contains(c, r)) {
						await _signal.Invoke<int, int>( "GetTerrain", c, r );
					}
				}
			}
		}

		public int GetTerrain( int column, int row ) {
			int chunkColumn = column / TerrainChunk.ChunkSize;
			int chunkRow = row / TerrainChunk.ChunkSize;

			if (_cache.Contains(chunkColumn, chunkRow)) {
				return 0;
			}

			return int.MinValue;
		}

		private void TerrainChunkCallback( TerrainChunk chunk ) {
			_cache.Put( chunk );
			if( _visibleArea.Contains(chunk.ChunkColumn, chunk.ChunkRow)) {
				_terrainChanged();
			}
		}
	}
}
