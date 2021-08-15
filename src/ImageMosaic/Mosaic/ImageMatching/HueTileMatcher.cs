using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.ImageMatching
{
	class HueTileMatcher : ITileMatcher
	{
		private (int hue, TileImage tile)[] _tiles;

		public TileImage FindMatch(MagickColor color)
		{
			return BinarySearch(color.GetHue());
		}

		public void SetTiles(IEnumerable<TileImage> tiles)
		{
			_tiles = tiles
				.Select(tile => (tile.Color.GetHue(), tile))
				.OrderBy(p => p.Item1).ToArray();
		}

		private TileImage BinarySearch(float hue)
		{
			int first = 0;
			int last = _tiles.Length - 1;
			int mid;

			do
			{
				mid = first + (last - first) / 2;
				if (hue > _tiles[mid].hue)
				{
					first = mid + 1;
				}
				else
				{
					last = mid - 1;
				}

				if (_tiles[mid].hue == hue)
				{
					return _tiles[mid].tile;
				}
			} 
			while (first <= last);

			return _tiles[mid].tile;
		}
	}
}
