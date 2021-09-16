using ImageMagick;
using Mosaic.Progress;
using Mosaic.Settings;
using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mosaic
{
	public class MosaicImage
	{
		private IPicture _mainImage;
		private readonly TileSources _sources;
		private readonly RenderSettings _settings;
		public IProgress<MosaicProgress> StepProgress { get; set; }
		public IProgress<TileProgress> TileProgress { get; set; }

		public MosaicImage(TileSources sources, RenderSettings settings)
		{
			_sources = sources;
			_settings = settings;

			if (!_settings.IsValid(out var message))
			{
				throw new ArgumentException(message);
			};
		}

		public Task SetMainImageAsync(string path)
		{
			return Task.Run(() =>
			{
				_mainImage = PictureFactory.Open(path);
				_mainImage.Resize(_settings.Columns * _settings.Resolution, _settings.Rows * _settings.Resolution);
			});
		}

		public async Task<RenderResult> RenderAsync()
		{
			var startedAt = DateTime.Now;

			await ResetTileSourceMetadata();

			Grid grid = new Grid(_settings.Columns, _settings.Rows, _settings.UseGridSearch);
			Matcher matcher = new(grid, _sources, _mainImage, _settings);
			Renderer renderer = new Renderer(grid, _settings.Resolution);

			matcher.TileProgress = TileProgress;
			await LoadSamplesAsync();
			await matcher.FindMatchesAsync();
			var render = await renderer.RenderAsync();

			return RenderResult.Build(startedAt, render, _sources);
		}

		private Task ResetTileSourceMetadata()
		{
			//Might be unnacessary to make this a task since it is fast
			return Task.Run(() =>
			{
				StepProgress?.Report(new MosaicProgress("Preparing metadata"));
				foreach (var tile in _sources)
				{
					tile.ResetMetadata();
				}
			});
		}

		private Task LoadSamplesAsync()
		{
			return Task.Run(() =>
			{
				StepProgress?.Report(new MosaicProgress("Indexing"));
				Parallel.ForEach(_sources, source =>
				{
					source.LoadSamples(_settings.SamplesPerTile, _settings.UseAverageSamples);
				});
			});
		}
	}
}
