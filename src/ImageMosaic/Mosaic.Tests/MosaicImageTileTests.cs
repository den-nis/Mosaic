using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mosaic;
using Mosaic.Graphics;
using Mosaic.Settings;
using Mosaic.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Tests
{
	[TestClass]
	public class MosaicImageTileTests
	{
		[TestMethod]
		public void TestMirror()
		{
			RenderSettings settings = new()
			{
				Size = .5f,
				UseMirror = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				" R",
				"RR"
			);

			using ImageMock tileImage = ImageMock.Build(
				"B ",
				"BB"
			);

			using ImageMock expectedImage = ImageMock.Build(
				" B",
				"BB"
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestMirrorAndRotation()
		{
			RenderSettings settings = new()
			{
				Size = 1.0f / 3.0f,
				UseMirror = true,
				UseRotation = true,
				SamplesPerTile = 9,
				Resolution = 3,
			};

			using ImageMock mainImage = ImageMock.Build(
				"RR ",
				" R ",
				" RR"
			);

			using ImageMock tileImage = ImageMock.Build(
				"B  ",
				"BBB",
				"  B"
			);

			using ImageMock expectedImage = ImageMock.Build(
				"BB ",
				" B ",
				" BB"
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestRightRotation()
		{
			RenderSettings settings = new()
			{
				Size = 0.5f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				" R",
				" R"
			);

			using ImageMock tileImage = ImageMock.Build(
				"BB",
				"  "
			);

			using ImageMock expectedImage = ImageMock.Build(
				" B",
				" B"
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestLeftRotation()
		{
			RenderSettings settings = new()
			{
				Size = 0.5f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R ",
				"R "
			);

			using ImageMock tileImage = ImageMock.Build(
				"BB",
				"  "
			);

			using ImageMock expectedImage = ImageMock.Build(
				"B ",
				"B "
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestNoRotation()
		{
			RenderSettings settings = new()
			{
				Size = 0.5f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R ",
				"R "
			);

			using ImageMock tileImage = ImageMock.Build(
				"B ",
				"B "
			);

			using ImageMock expectedImage = ImageMock.Build(
				"B ",
				"B "
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestComplexShapeRotation()
		{
			RenderSettings settings = new()
			{
				Size = 1.0f / 3.0f,
				UseRotation = true,
				UseMirror = false,
				SamplesPerTile = 9,
				Resolution = 3,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R  ",
				"RR ",
				"RRR"
			);

			using ImageMock tileImage = ImageMock.Build(
				" BB",
				"  B",
				"   "
			);

			using ImageMock expectedImage = ImageMock.Build(
				"   ",
				"B  ",
				"BB "
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestMultipleOverlap()
		{
			RenderSettings settings = new()
			{
				Size = 0.5f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R ",
				"RR"
			);

			using ImageMock tileImage = ImageMock.Build(
				" B",
				"BB"
			);

			using ImageMock expectedImage = ImageMock.Build(
				"B ",
				"BB"
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestMultipleChoice()
		{
			RenderSettings settings = new()
			{
				Size = 0.5f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 2,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R ",
				"  "
			);

			using ImageMock tileImage = ImageMock.Build(
				" B",
				"B "
			);

			using ImageMock expectedImage = ImageMock.Build(
				"B ",
				" B"
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public void TestNearMatch()
		{
			RenderSettings settings = new()
			{
				Size = 1.0f / 8.0f,
				UseRotation = true,
				SamplesPerTile = 4,
				Resolution = 8,
				UseAverageSamples = true,
			};

			using ImageMock mainImage = ImageMock.Build(
				"R       ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        "
			);

			using ImageMock tileImage = ImageMock.Build(
				"        ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        ",
				"      R ",
				"        "
			);

			using ImageMock expectedImage = ImageMock.Build(
				"        ",
				" R      ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        ",
				"        "
			);

			using ImageMock result = QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		private static ImageMock QuickRender(ImageMock tile, ImageMock main, RenderSettings settings)
		{
			using Stream tileStream = tile.GetMemoryStream();
			using Stream mainStream = main.GetMemoryStream();

			MosaicImage mosaic = new(settings)
			{
				MainPicture = new PictureSourceStream(mainStream, "Main"),
				TilePictures = new[] { new PictureSourceStream(tileStream) }
			};

			using MemoryStream result = new();
			mosaic.Render().Write(result);
			result.Position = 0;

			ImageMock mock = new();
			mock.SetFromStream(result);
			return mock;
		}
	}
}
