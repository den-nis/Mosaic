using ImageMagick;
using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class RenderResult : IDisposable
	{
		public DateTime StartedAt { get; private set; }
		public DateTime FinishedAt { get; private set; }
		public TimeSpan Time => FinishedAt - StartedAt;
		public Dictionary<string, int> TileUsageOverview { get; private set; }

		public int TotalPictures { get; private set; } 
		public int UsedPictures { get; private set; } 

		private IPicture _picture;

		internal static RenderResult Build(DateTime startedAt, IPicture picture, TileSet set)
		{
			return new RenderResult()
			{
				StartedAt = startedAt,
				FinishedAt = DateTime.Now,
				_picture = picture,
				TileUsageOverview = set.Tiles.ToDictionary(k => k.Source.Identifier, v => v.TimesUsed),
				TotalPictures = set.Tiles.Count(),
				UsedPictures = set.Tiles.Where(t => t.TimesUsed > 0).Count()
			};
		}

		private RenderResult()
		{

		}

		public void Write(string path) => _picture.Write(path);
		
		public void Write(Stream stream) => _picture.Write(stream);
		
		public void Dispose()
		{
			_picture.Dispose();
		}
	}
}
