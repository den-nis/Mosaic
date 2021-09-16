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
		public ResultPicture Picture { get; private set; }
		public Dictionary<TileSource, int> TileUsageOverview { get; private set; }

		internal static RenderResult Build(DateTime startedAt, IPicture picture, TileSources sources)
		{
			return new RenderResult()
			{
				StartedAt = startedAt,
				FinishedAt = DateTime.Now,
				Picture = new ResultPicture(picture),
				TileUsageOverview = sources.ToDictionary(k => k, v => v.TimesUsed),
			};
		}

		private RenderResult()
		{

		}

		public void Dispose()
		{
			Picture.Dispose();
		}
	}
}
