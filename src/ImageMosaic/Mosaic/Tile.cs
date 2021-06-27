using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class Tile : IDisposable
	{
		public string ImagePath { get; set; }

		public TileMode TileMode { get; set; }

		public MagickImage Thumbnail { get; private set; }
		public MagickColor Color { get; private set; }

		public Tile(string imagePath)
		{
			ImagePath = imagePath;
		}

		public void CreateThumbnail(int size)
		{
			var thumbnail = new MagickImage(ImagePath);
			thumbnail.ConvertToSquare(TileMode);
			thumbnail.Resize(size, size);
			Thumbnail = thumbnail;
		}

		public void ComputeAverageColor()
		{
			using var image = new MagickImage(ImagePath);
			Color = image.AverageColor();
		}

		public void Dispose()
		{
			Thumbnail.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
