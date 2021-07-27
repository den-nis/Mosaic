using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public static class MagickImageExtensions
	{
		public static void ConvertToSquare(this MagickImage image, TileImageMode tileMode)
		{
			switch(tileMode)
			{
				case TileImageMode.Center:
					ConvertToSquareCenter(image);
					break;

				case TileImageMode.Stretch:
					ConvertToSquareStretch(image);
					break;

				default:
					throw new KeyNotFoundException();
			}
		}

		private static void ConvertToSquareStretch(MagickImage image)
		{
			var size = Math.Max(image.Width, image.Height);
			image.Resize(new MagickGeometry
			{
				Width = size,
				Height = size,
				IgnoreAspectRatio = true,
			});
		}

		private static void ConvertToSquareCenter(MagickImage image)
		{
			int x = 0, y = 0;
			int size;

			if (image.Width > image.Height)
			{
				x = (image.Width - image.Height) / 2;
				size = image.Height;
			}
			else
			{
				y = (image.Height - image.Width) / 2;
				size = image.Width;
			}

			image.Crop(new MagickGeometry
			{
				Y = y, X = x,
				Width = size, 
				Height = size,
				IgnoreAspectRatio = true,
			});
		}

		public static MagickColor GetAverageColor(this MagickImage image)
		{
			long totalRed = 0;
			long totalBlue = 0;
			long totalGreen = 0;

			using var pixels = image.GetPixels();
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					var color = pixels[x, y].ToColor();
					totalRed += color.R;
					totalGreen += color.G;
					totalBlue += color.B;
				}
			}

			var pixelCount = image.Width * image.Height;

			return new MagickColor(
				(ushort)(totalRed    / pixelCount),
				(ushort)(totalGreen / pixelCount),
				(ushort)(totalBlue / pixelCount)
			);
		}
	}
}
