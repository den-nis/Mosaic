using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Tests
{
	static class ImageAssert
	{
		public static void AreEqual(MagickImage expected, MagickImage actual)
		{
			Assert.AreEqual(expected.Width, actual.Width, "Image width");
			Assert.AreEqual(expected.Height, actual.Height, "Image height");

			using var expectedPixels = expected.GetPixels();
			using var actualPixels = actual.GetPixels();

			for (int y = 0; y < expected.Height; y++)
			{
				for (int x = 0; x < expected.Width; x++)
				{
					Assert.AreEqual(expectedPixels[x, y], actualPixels[x, y], "Incorrect pixel");
				}
			}
		}
	}
}
