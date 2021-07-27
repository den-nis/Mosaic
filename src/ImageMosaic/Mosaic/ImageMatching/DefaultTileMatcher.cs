using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.ImageMatching
{
	class DefaultTileMatcher : ITileMatcher
	{
		private TileImage[] _tiles;

		public void SetTiles(IEnumerable<TileImage> tiles)
		{
			_tiles = tiles.ToArray();
		}

		public TileImage FindMatch(MagickColor color)
		{
			return DetermineTile(color);
		}

		private TileImage DetermineTile(MagickColor color)
		{
			Debug.Assert(_tiles != null, "SetTiles() was not called");

			TileImage leastDifferentTile = null;
			int leastDifference = int.MaxValue;

			foreach (var tile in _tiles)
			{
				var difference = ColorDifference(tile.Color, color);
				if (difference < leastDifference)
				{
					leastDifferentTile = tile;
					leastDifference = difference;
				}
			}

			return leastDifferentTile;
		}

		/// <summary>
		/// The higher the return value the more different the colors are
		/// </summary>
		private static int ColorDifference(MagickColor colorA, MagickColor colorB)
		{
			var r = Math.Abs(colorA.R - colorB.R);
			var g = Math.Abs(colorA.G - colorB.G);
			var b = Math.Abs(colorA.B - colorB.B);

			return r + g + b;
		}
	}
}
