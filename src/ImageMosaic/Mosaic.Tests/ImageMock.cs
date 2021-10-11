using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Tests
{
	public class ImageMock : IDisposable
	{
		private MagickImage _internalImage;
		private static Random _rng = new Random();

		public static ImageMock Build(params string[] data)
		{
			var mock = new ImageMock();
			mock.BuildImage(data);
			return mock;
		}

		public static ImageMock Noise(int width, int height)
		{
			var mock = new ImageMock();
			mock.BuildImageNoise(width, height);
			return mock;
		}

		public void SetFromStream(Stream stream)
		{
			_internalImage = new MagickImage(stream);
		}

		public Stream GetMemoryStream()
		{
			MemoryStream stream = new();
			_internalImage.Write(stream, MagickFormat.Png);
			stream.Position = 0;

			return stream;
		}

		private void BuildImageNoise(int width, int height)
		{ 
			MagickImage image = new MagickImage(MagickColors.Black, width, height);
			using var pixels = image.GetPixels();

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					byte[] bytes = new byte[3];
					_rng.NextBytes(bytes);
					pixels.SetPixel(x, y, bytes);
				}
			}

			_internalImage = image;
		}

		private void BuildImage(params string[] data)
		{
			MagickImage image = new MagickImage(MagickColors.Black, data[0].Length, data.Length);
			using var pixels = image.GetPixels();

			for (int y = 0; y < data.Length; y++)
			{
				for (int x = 0; x < data[0].Length; x++)
				{
					var c = ToColor(data[y][x]);
					pixels.SetPixel(x, y, new[] { c.R, c.G, c.B });
				}
			}

			_internalImage = image;
		}

		private MagickColor ToColor(char c)
		{
			switch (c)
			{
				case 'W': return new MagickColor(255, 255, 255);
				case 'R': return new MagickColor(255, 0, 0);
				case 'G': return new MagickColor(0, 255, 0);
				case 'B': return new MagickColor(0, 0, 255);
				case ' ': return new MagickColor(0, 0, 0);
				default: throw new InvalidOperationException("Unknown color");
			}
		}

		public void AssertEquals(ImageMock expected)
		{
			Assert.AreEqual(expected._internalImage.Width, _internalImage.Width, "Image width");
			Assert.AreEqual(expected._internalImage.Height, _internalImage.Height, "Image height");

			using var expectedPixels = expected._internalImage.GetPixels();
			using var actualPixels = _internalImage.GetPixels();

			for (int y = 0; y < _internalImage.Height; y++)
			{
				for (int x = 0; x < _internalImage.Width; x++)
				{
					Assert.AreEqual(expectedPixels[x, y][0], actualPixels[x, y][0], $"Incorrect red pixel at {x}x{y}");
					Assert.AreEqual(expectedPixels[x, y][1], actualPixels[x, y][1], $"Incorrect green pixel {x}x{y}");
					Assert.AreEqual(expectedPixels[x, y][2], actualPixels[x, y][2], $"Incorrect blue pixel {x}x{y}");
				}
			}
		}

		public void Dispose()
		{
			_internalImage?.Dispose();
		}
	}
}
