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
	public class TileImageCollection : IEnumerable<TileImage>
	{
		private readonly ConcurrentBag<TileImage> _tileImages = new();

		public void AddAllFromFolder(string path, string searchPattern)
		{
			Parallel.ForEach(Directory.GetFiles(path, searchPattern), file =>
			{
				Add(file);
			});
		}

		public void Add(string file)
		{
			var image = new TileImage(file);
			_tileImages.Add(image);
		}

		public void Add(TileImage tileImage) => _tileImages.Add(tileImage);

		public IEnumerator<TileImage> GetEnumerator() => _tileImages.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
