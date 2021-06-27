using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileGrid
	{
		public ParallelOptions ParallelOptions { get; set; } = new ParallelOptions();
		private readonly CancellationToken _cancellationToken = new();

		public Tile[,] Grid { get; private set; }
		private readonly object _tileGridLock = new();
		private TileCollection _tiles;
		private MagickImage _main = null;

		public void SetTileSource(TileCollection tiles)
		{
			_tiles = tiles;
		}

		public void SetMainImage(MagickImage image)
		{
			Grid = new Tile[image.Width, image.Height];
			_main = image;
		}

		public Task DetermineLocationsAsync()
		{
			return Task.Run(() => DetermineLocations(), _cancellationToken);	
		}

		public void DetermineLocations()
		{
			using var pixels = _main.GetPixels();

			ParallelOptions.CancellationToken = _cancellationToken;
			Parallel.For(0, _main.Height, ParallelOptions, y =>
			{
				for (int x = 0; x < _main.Width; x++)
				{
					DetermineAndSetTile(x, y, (MagickColor)pixels[x, y].ToColor());
				}
			});
		}

		private void DetermineAndSetTile(int x, int y, MagickColor color)
		{
			var tile = DetermineTile(color);
			SetTile(x, y, tile);
		}

		public void SetTile(int x, int y, Tile tile)
		{
			lock (_tileGridLock)
			{
				Grid[x, y] = tile;
			}
		}

		private Tile DetermineTile(MagickColor color)
		{
			Tile bestTile = null;
			double bestMatch = -1; //%

			foreach (var tile in _tiles)
			{
				var similarity = ColorMatch(tile.Color, color);
				if (similarity > bestMatch)
				{
					bestTile = tile;
					bestMatch = similarity;
				}
			}

			return bestTile;
		}

		/// <summary>
		/// Percentage in how similar the colors are
		/// </summary>
		private static double ColorMatch(MagickColor colorA, MagickColor colorB)
		{
			var r = Math.Abs(colorA.R - colorB.R);
			var g = Math.Abs(colorA.G - colorB.G);
			var b = Math.Abs(colorA.B - colorB.B);

			var difference = ushort.MaxValue - (r + g + b);
			var percentage = difference / (double)ushort.MaxValue;
			return percentage;
		}
	}
}
