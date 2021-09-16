using Mosaic.Progress;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileSources : IEnumerable<TileSource>
	{
		public int Resolution { get; }

		private readonly ConcurrentDictionary<TileSource, byte> _sources = new ConcurrentDictionary<TileSource, byte>();

		public TileSources(int resolution)
		{
			Resolution = resolution;
		}

		public Task AddAsync(TileSource tile)
		{
			return AddRangeAsync(new[] { tile }, null);
		}

		public Task AddRangeAsync(IEnumerable<TileSource> tiles, IProgress<FileProgress> progress)
		{
			return LoadAsync(AddNew(tiles), progress);
		}

		public Task SetAsync(IEnumerable<TileSource> tiles, IProgress<FileProgress> progress)
		{
			var lookup = new HashSet<TileSource>(tiles.Distinct());
			foreach (var item in _sources)
			{
				if (!lookup.Contains(item.Key))
				{
					_sources.TryRemove(item.Key, out _);
				}
			}

			return LoadAsync(AddNew(lookup), progress);
		}

		private Task LoadAsync(IEnumerable<TileSource> tiles, IProgress<FileProgress> progress)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(tiles, tile =>
				{
					progress?.Report(new FileProgress(tile.FullPath));
					tile.Load(Resolution);
				});
			});
		}

		/// <returns>Returns the new elements</returns>
		private IEnumerable<TileSource> AddNew(IEnumerable<TileSource> sources)
		{
			foreach (var item in sources.Distinct())
			{
				if (_sources.TryAdd(item, 0))
				{
					yield return item;
				}
			}
		}

		public IEnumerator<TileSource> GetEnumerator()
		{
			return _sources.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _sources.GetEnumerator();
		}
	}
}
