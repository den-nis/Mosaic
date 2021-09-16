using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Progress
{
	public record FileProgress
	{
		public string Image { get; }

		public FileProgress(string image)
		{
			Image = image;
		}

		public override string ToString()
		{
			return Image;
		}
	}
}
