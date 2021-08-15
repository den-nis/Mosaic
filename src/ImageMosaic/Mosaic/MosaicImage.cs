using ImageMagick;
using Microsoft.Extensions.Caching.Memory;
using Mosaic.ImageMatching;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	public class MosaicImage
	{
		public float OverlayOpacity { get; set; } = 0f;
		public CompositeOperator OverlayOperator { get; set; } = CompositeOperator.Over;

		public IProgress<MosaicProgress> Progress { get; init; }
		public IEnumerable<string> Files { get; init; }
		public TileImageMode TileMode { get; init; }
		public string Main { get; init; }

		public int Resolution { get; init; }
		public int Rows { get; init; }
		public int Cols { get; init; }

		public int Contrast { get; init; }
		public int Brightness { get; init; }
		public int Red { get; init; }
		public int Green { get; init; }
		public int Blue { get; init; }

		public async Task<RenderResult> Render()
		{
			var renderStart = DateTime.Now;

			ValidateParameters();

			Progress?.Report(new MosaicProgress("Loading"));
			using var main = await LoadMainImageAsync();
			await ApplyEffects(main);

			var images = CreateTileImages();
			await LoadTileImages(images);

			Progress?.Report(new MosaicProgress("Finding matches"));
			var grid = new TileGrid(images, main);
			await grid.DetermineLocationsAsync();
			var image = await Render(grid);

			Progress?.Report(new MosaicProgress("Cleanup"));
			images.ForEach(i => i.Dispose());

			Progress?.Report(new MosaicProgress("Finished", 1));
			return new RenderResult
			{
				StartedAt = renderStart,
				FinishedAt = DateTime.Now,
				ImageStream = image,
			};
		}

		private void ValidateParameters()
		{
			if (!Files.Any())
				throw new InvalidOperationException("Missing images");

			if (string.IsNullOrEmpty(Main))
				throw new InvalidOperationException("Missing main image");
		}

		private async Task<Stream> Render(TileGrid grid)
		{
			var renderer = new TileGridRenderer(grid, Resolution);
			using var render = await renderer.RenderAsync(Progress);

			Progress?.Report(new MosaicProgress("Writing"));

			return await Task.Run(() =>
			{
				var stream = new MemoryStream();
				render.Write(stream, MagickFormat.Png);
				return stream;
			});
		}

		private Task<MagickImage> LoadMainImageAsync()
		{
			return Task.Run(() =>
			{
				var main = new MagickImage(Main);
				main.Resize(new MagickGeometry
				{
					IgnoreAspectRatio = true,
					Width = Cols,
					Height = Rows,
				});

				return main;
			});
		}

		private List<TileImage> CreateTileImages()
		{
			return Files.Select(f => new TileImage(f)).ToList();
		}

		private Task LoadTileImages(IEnumerable<TileImage> images)
		{
			return Task.Run(() =>
			{
				Parallel.ForEach(images, tile =>
				{
					tile.Load(Resolution, TileMode);
				});
			});
		}

		public Task ApplyEffects(MagickImage image)
		{
			return Task.Run(() =>
			{
				image.Tint(0.5f.ToString(CultureInfo.InvariantCulture), new MagickColor((ushort)Red, (ushort)Green, (ushort)Blue));
				image.BrightnessContrast(new Percentage(Brightness), new Percentage(Contrast));
			});
		}
	}
}
