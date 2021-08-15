using ImageMagick;
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
		public DateTime StartedAt { get; internal set; }
		public DateTime FinishedAt { get; internal set; }
		public Stream ImageStream { get; internal set; }

		public void Dispose()
		{
			ImageStream.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
