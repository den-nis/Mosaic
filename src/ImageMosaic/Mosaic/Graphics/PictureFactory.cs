using Mosaic.Graphics.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics
{
	internal static class PictureFactory
	{
		public static IPicture Create(int width, int height)
		{
			return new MagickPicture(width, height);
		}

		public static IPicture Open(string filename)
		{
			return new MagickPicture(filename);
		}
	}
}
