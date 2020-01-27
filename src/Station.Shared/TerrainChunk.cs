using System;
using System.Collections.Generic;
using System.Text;

namespace Station.Shared {
	public class TerrainChunk {

		public const int ChunkSize = 10;

		public TerrainChunk() {
			Terrain = new int[ChunkSize * ChunkSize];
			ChunkColumn = int.MinValue;
			ChunkRow = int.MinValue;
		}

		public int ChunkColumn { get; set; }

		public int ChunkRow { get; set; }

		public int[] Terrain { get; set; }
	}
}
