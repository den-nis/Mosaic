﻿using ImageMagick;
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
		private static readonly Random _rng = new();

		public static ImageMock Build(params string[] data)
		{
			ImageMock mock = new();
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
			MagickImage image = new(MagickColors.Black, width, height);
			using IPixelCollection<byte> pixels = image.GetPixels();

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
			MagickImage image = new(MagickColors.Black, data[0].Length, data.Length);
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

		private static MagickColor ToColor(char c)
		{
			return c switch
			{
				'W' => new MagickColor(255, 255, 255),
				'R' => new MagickColor(255, 0, 0),
				'G' => new MagickColor(0, 255, 0),
				'B' => new MagickColor(0, 0, 255),
				' ' => new MagickColor(0, 0, 0),
				_ => throw new InvalidOperationException("Unknown color"),
			};
		}

		public void AssertEquals(ImageMock expected)
		{
			Assert.AreEqual(expected._internalImage.Width, _internalImage.Width, "Image width");
			Assert.AreEqual(expected._internalImage.Height, _internalImage.Height, "Image height");

			using IPixelCollection<byte> expectedPixels = expected._internalImage.GetPixels();
			using IPixelCollection<byte> actualPixels = _internalImage.GetPixels();

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
			GC.SuppressFinalize(this);
		}
	}
}
