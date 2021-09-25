using Mosaic.Progress;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileSet : IDisposable
	{
		public int Resolution { get; }
		public CropMode CropMode { get; }

		/// <summary>
		/// The value of this dictionary is not used.
		/// </summary>
		internal ConcurrentDictionary<Tile, byte> Sources { get; set; } = new();
		internal IEnumerable<Tile> Tiles => Sources.Keys;

		public TileSet(int resolution, CropMode cropMode)
		{
			Resolution = resolution;
			CropMode = cropMode;
		}

		public Task LoadTilesAsync(IEnumerable<PictureSource> sources, IProgress<FileProgress> progress)
		{
			var tiles = sources.Select(s => new Tile(s));
			return AddRangeAsync(tiles, progress);
		}

		internal void ResetMetadata()
		{
			foreach(var tile in Sources.Keys)
			{
				tile.ResetMetadata();
			}
		}

		private Task AddAsync(Tile tile)
		{
			return AddRangeAsync(new[] { tile }, null);
		}

		private Task AddRangeAsync(IEnumerable<Tile> tiles, IProgress<FileProgress> progress)
		{
			return LoadAsync(AddNew(tiles), progress);
		}

		private Task SetAsync(IEnumerable<Tile> tiles, IProgress<FileProgress> progress)
		{
			var lookup = new HashSet<Tile>(tiles.Distinct());
			foreach (var item in Sources)
			{
				if (!lookup.Contains(item.Key))
				{
					Sources.TryRemove(item.Key, out _);
				}
			}

			return LoadAsync(AddNew(lookup), progress);
		}

		private Task LoadAsync(IEnumerable<Tile> tiles, IProgress<FileProgress> progress)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(tiles, tile =>
				{
					progress?.Report(new FileProgress(tile.Source.Identifier));
					tile.Load(Resolution, CropMode);
				});
			});
		}

		/// <returns>Returns the new elements</returns>
		private IEnumerable<Tile> AddNew(IEnumerable<Tile> sources)
		{
			foreach (var item in sources.Distinct())
			{
				if (Sources.TryAdd(item, 0))
				{
					yield return item;
				}
			}
		}

		public void Dispose()
		{
			foreach(var tile in Tiles)
			{
				tile?.Dispose();
			}
		}
	}
}
