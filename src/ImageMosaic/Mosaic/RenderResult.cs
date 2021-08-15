using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class RenderResult
	{
		public DateTime StartedAt { get; internal set; }
		public DateTime FinishedAt { get; internal set; }
		public Stream ImageStream { get; internal set; }
	}
}
