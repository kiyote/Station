using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Station.Shared;

namespace Station.Client.Services.Map {
	internal sealed class TerrainCache {

		private readonly Dictionary<long, TerrainChunk> _chunks;

		public TerrainCache() {
			_chunks = new Dictionary<long, TerrainChunk>();
		}

		public bool Contains( int chunkColumn, int chunkRow ) {
			long key = ChunkToKey( chunkColumn, chunkRow );

			return _chunks.ContainsKey( key );
		}

		public void Put( TerrainChunk chunk ) {
			long key = ChunkToKey( chunk.ChunkColumn, chunk.ChunkRow );
			_chunks[key] = chunk;
		}

		public TerrainChunk Get( int chunkColumn, int chunkRow ) {
			long key = ChunkToKey( chunkColumn, chunkRow );
			return _chunks[key];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long ChunkToKey( long column, long row ) {
			return ( column << 32 ) | row;
		}
	}
}
