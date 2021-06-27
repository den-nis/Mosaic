using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileCollection : IEnumerable<Tile>
	{
		private readonly ConcurrentBag<Tile> _tiles = new();

		public bool InitializeOnAdd { get; set; } = true;

		public void AddAllFromFolder(string path, string searchPattern)
		{
			Parallel.ForEach(Directory.GetFiles(path, searchPattern), file =>
			{
				Add(file);
			});
		}

		public void Add(string file)
		{
			var tile = new Tile(file);

			if (InitializeOnAdd)
				tile.Initialize();

			_tiles.Add(tile);
		}

		public void Add(Tile tile) => _tiles.Add(tile);

		public IEnumerator<Tile> GetEnumerator() => _tiles.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
