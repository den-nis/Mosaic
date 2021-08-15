using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	class TileImage : IDisposable
	{
		public MagickColor Color { get; private set; }
		public MagickImage Image { get; private set; }
		public int Resolution => Image?.Width ?? -1;

		public string ImagePath { get; private set; }
		private bool _isLoaded = false;

		public TileImage(string imagePath)
		{
			ImagePath = imagePath;
		}

		public void Load(int resolution, TileImageMode tileMode = TileImageMode.Center)
		{
			if (_isLoaded) throw new InvalidOperationException("TileImage is already loaded");
			_isLoaded = true;

			Image = LoadImage(resolution, tileMode);

			ComputeAverageColor(Image);
		}

		private MagickImage LoadImage(int resolution, TileImageMode tileMode)
		{
			var image = new MagickImage(ImagePath);
			ToSquare(image, resolution, tileMode);
			return image;
		}

		private static void ToSquare(MagickImage target, int size, TileImageMode tileMode)
		{
			target.ConvertToSquare(tileMode);
			target.Resize(size, size);
		}

		private void ComputeAverageColor(MagickImage image)
		{
			Color = image.GetAverageColor();
		}

		public void Dispose()
		{
			Image?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
