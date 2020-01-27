using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Station.Shared {
	public struct Rect {

		public Rect(
			int x,
			int y,
			int w,
			int h
		) {
			X = x;
			Y = y;
			W = w;
			H = h;
		}

		public bool Contains( int chunkColumn, int chunkRow ) {
			int tx = chunkColumn * TerrainChunk.ChunkSize;
			int ty = chunkRow * TerrainChunk.ChunkSize;

			bool xOverlap = ValueInRange( X, tx, tx + TerrainChunk.ChunkSize )
				|| ValueInRange( tx, X, X + W );

			bool yOverlap = ValueInRange( Y, ty, ty + TerrainChunk.ChunkSize )
				|| ValueInRange( ty, Y, Y + H );

			return xOverlap && yOverlap;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		private static bool ValueInRange(int value, int min, int max ) {
			return ( value >= min ) && ( value <= max );
		}

		public int X;

		public int Y;

		public int W;

		public int H;
	}
}
