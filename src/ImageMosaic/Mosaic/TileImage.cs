using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileImage : IDisposable
	{
		public TileImageMode TileMode { get; set; } = TileImageMode.Center;
		public MagickColor Color { get; private set; }

		private readonly string _imagePath;

		public TileImage(string imagePath)
		{
			_imagePath = imagePath;
		}

		public MagickImage Thumbnail { get; private set; }
		public MagickImage Image { get; private set; }

		public void Initialize(int renderResolution, int thumbnailResolution)
		{
			Image = Render(renderResolution);
			Thumbnail = Render(thumbnailResolution);
			
			ComputeAverageColor(Image);
		}

		private MagickImage Render(int resolution)
		{
			var image = new MagickImage(_imagePath);
			ToSquare(image, resolution);
			return image;
		}

		private void ToSquare(MagickImage target, int size)
		{
			target.ConvertToSquare(TileMode);
			target.Resize(size, size);
		}

		private void ComputeAverageColor(MagickImage image)
		{
			Color = image.GetAverageColor();
		}

		public void Dispose()
		{
			Image.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
