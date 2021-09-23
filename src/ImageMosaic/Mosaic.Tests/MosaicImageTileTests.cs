using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mosaic.Graphics;
using Mosaic.Settings;
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
		public async Task TestMirror()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestMirrorAndRotation()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestRightRotation()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestLeftRotation()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestNoRotation()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestComplexShapeRotation()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestMultipleOverlap()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestMultipleChoice()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		[TestMethod]
		public async Task TestNearMatch()
		{
			RenderSettings settings = new RenderSettings
			{
				Columns = 1,
				Rows = 1,
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

			using ImageMock result = await QuickRender(tileImage, mainImage, settings);
			result.AssertEquals(expectedImage);
		}

		private async Task<ImageMock> QuickRender(ImageMock tile, ImageMock main, RenderSettings settings)
		{
			using var tileStream = tile.GetMemoryStream();
			using var mainStream = main.GetMemoryStream();

			TileSet sources = new TileSet(settings.Resolution, CropMode.Center);
			await sources.LoadTilesAsync(new[] { new PictureSourceStream(tileStream) }, null);

			MosaicImage mosaic = new MosaicImage(sources, settings);
			await mosaic.SetMainImageAsync(new PictureSourceStream(mainStream, "Main"));

			using MemoryStream result = new();
			(await mosaic.RenderAsync()).Write(result);
			result.Position = 0;

			ImageMock mock = new();
			mock.SetFromStream(result);
			return mock;
		}
	}
}
