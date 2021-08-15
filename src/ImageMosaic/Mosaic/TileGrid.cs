using ImageMagick;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Mosaic.ImageMatching;

namespace Mosaic
{
	class TileGrid
	{
		public ITileMatcher TileMatcher { get; set; } = new DefaultTileMatcher();

		private readonly IEnumerable<TileImage> _tiles;
		private readonly TileImage[,] _grid;
		private readonly MagickImage _main = null;

		public TileImage this[int x, int y] => _grid[x, y];

		public int Width => _grid.GetLength(0);
		public int Height => _grid.GetLength(1);

		private readonly HashSet<TileImage> _usedTiles = new();

		public TileGrid(IEnumerable<TileImage> tiles, MagickImage main)
		{
			_tiles = tiles;
			_main = main;
			_grid = new TileImage[main.Width, main.Height];
		}
		public bool ContainsTileImage(TileImage image)
		{
			return _usedTiles.Contains(image);
		}

		public Task DetermineLocationsAsync() => Task.Run(() => DetermineLocations());

		private void DetermineLocations()
		{
			_usedTiles.Clear();
			TileMatcher.SetTiles(_tiles);
			using var pixels = _main.GetPixels();

			for (int y = 0; y < _main.Height; y++)
			{
				for (int x = 0; x < _main.Width; x++)
				{
					var tile = TileMatcher.FindMatch((MagickColor)pixels[x, y].ToColor());

					if (!_usedTiles.Contains(tile))
					{
						_usedTiles.Add(tile);
					}

					SetTile(x, y, tile);
				}
			}
		}

		public void SetTile(int x, int y, TileImage tile)
		{
			_grid[x, y] = tile;
		}
	}
}
