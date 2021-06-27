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

		public TileMode TileMode { get; set; } = TileMode.Center;
		public int ThumbnailResolution { get; set; } = 16;
		public bool FullImageAverage { get; set; } = false;

		public MagickImage Thumbnail { get; private set; }
		public MagickColor Color { get; private set; }

		public Tile(string imagePath)
		{
			ImagePath = imagePath;
		}

		public void Initialize()
		{
			var image = new MagickImage(ImagePath);
			CreateThumbnail(image);
			ComputeAverageColor(FullImageAverage ? image : Thumbnail);
		}

		private void ComputeAverageColor(MagickImage image)
		{
			Color = image.GetAverageColor();
		}

		private void CreateThumbnail(MagickImage image)
		{
			var thumbnail = new MagickImage(image);
			ToSquare(thumbnail, ThumbnailResolution);
			Thumbnail = thumbnail;
		}

		public MagickImage RenderFinalImage(int size)
		{
			var image = new MagickImage(ImagePath);
			ToSquare(image, size);
			return image;
		}

		private void ToSquare(MagickImage target, int size)
		{
			target.ConvertToSquare(TileMode);
			target.Resize(size, size);
		}

		public void Dispose()
		{
			Thumbnail.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
