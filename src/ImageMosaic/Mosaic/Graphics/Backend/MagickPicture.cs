using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics.Backend
{
	internal class MagickPicture : IPicture
	{
		private readonly MagickImage _internalImage;
		private IUnsafePixelCollection<byte> _pixelCache;

		public int Width => _internalImage.Width;
		public int Height => _internalImage.Height;

		public MagickPicture(MagickPicture copy)
		{
			_internalImage = new MagickImage(copy._internalImage);
		}

		public MagickPicture(string filename)
		{
			_internalImage = new MagickImage(filename);
		}

		public MagickPicture(int width, int height)
		{
			_internalImage = new MagickImage(MagickColors.Black, width, height);
		}

		public void Resize(int width, int height)
		{
			_internalImage.Resize(new MagickGeometry
			{
				Width = width,
				Height = height,
				IgnoreAspectRatio = true
			});
		}

		public Sample[] ComputeSamples(int amount, bool useAverage)
		{
			return ComputeSamples(amount, useAverage, _internalImage.Width, 0, 0);
		}

		public Sample[] ComputeSamples(int amount, bool useAverage, int size, int offsetX, int offsetY)
		{
			List<Sample> result = new(amount);
			int line = (int)Math.Sqrt(amount);
			int blockSize = (int)(size / line);
			var pixels = _pixelCache ?? _internalImage.GetPixelsUnsafe();

			for (int by = 0; by < line; by++)
			{
				for (int bx = 0; bx < line; bx++)
				{
					int x = (offsetX + bx * blockSize);
					int y = (offsetY + by * blockSize);

					int centerX = (x + blockSize / 2);
					int centerY = (y + blockSize / 2);

					Color color;
					if (useAverage)
					{
						color = GetBlockAverageColor(
							pixels,
							x,
							y,
							blockSize);
					}
					else
					{
						var pixel = pixels[centerX, centerY].ToColor();
						color = Color.FromByteArray(pixel.ToByteArray());
					}

					result.Add(new Sample(centerX - offsetX, centerY - offsetY, color));
				}
			}

			return result.ToArray();
		}

		private static Color GetBlockAverageColor(IUnsafePixelCollection<byte> pixels, int offsetX, int offsetY, int size)
		{
			long totalGreen = 0, totalRed = 0, totalBlue = 0;

			for (int iy = 0; iy < size; iy++)
			{
				for (int ix = 0; ix < size; ix++)
				{
					var color = pixels[offsetX + ix, offsetY + iy].ToColor();
					totalRed += color.R;
					totalGreen += color.G;
					totalBlue += color.B;
				}
			}

			int pixelCount = size * size;

			return new Color(
				(byte)(totalRed / pixelCount),
				(byte)(totalGreen / pixelCount),
				(byte)(totalBlue / pixelCount)
			);
		}

		public void ApplyCropMode(CropMode cropMode)
		{
			switch (cropMode)
			{
				case CropMode.Center:
					ConvertToSquareCenter(_internalImage);
					break;

				case CropMode.Stretch:
					ConvertToSquareStretch(_internalImage);
					break;

				default:
					throw new KeyNotFoundException($"No case for {cropMode}");
			}
		}

		private static void ConvertToSquareStretch(MagickImage image)
		{
			int size = Math.Max(image.Width, image.Height);
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
				Y = y,
				X = x,
				Width = size,
				Height = size,
				IgnoreAspectRatio = true,
			});
		}

		public void WriteToStream(Stream stream)
		{
			_internalImage.Write(stream);
		}

		public void Write(string file)
		{
			_internalImage.Write(file);
		}

		public void Rotate(int angle)
		{
			_internalImage.Rotate(angle);
		}

		public void Mirror()
		{
			_internalImage.Flop();
		}

		public void Composite(IPicture img, int x, int y)
		{
			_internalImage.Composite((img as MagickPicture)._internalImage, x, y, CompositeOperator.Over);
		}

		public void CachePixels()
		{
			_pixelCache = _internalImage.GetPixelsUnsafe();
		}

		public void UncachePixels()
		{
			_pixelCache?.Dispose();
			_pixelCache = null;
		}

		public void Dispose()
		{
			_internalImage.Dispose();
			UncachePixels();
		}

		IPicture IPicture.Copy()
		{
			return new MagickPicture(this);
		}
	}
}
