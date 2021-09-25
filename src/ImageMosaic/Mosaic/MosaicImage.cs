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
using System.Threading;

namespace Mosaic
{
	public class MosaicImage : IDisposable
	{
		public bool DisposeMainImage { get; set; } = true;

		private readonly TileSet _set;
		private readonly RenderSettings _settings;
		private Stream _mainPictureStream;
		private IPicture _mainPicture;

		public MosaicImage(TileSet sources, RenderSettings settings)
		{
			_set = sources;
			_settings = settings;

			if (!_settings.IsValid(out var message))
			{
				throw new ArgumentException(message);
			}
		}

		public Task SetMainImageAsync(PictureSource source)
		{
			_mainPictureStream = source.GetDataStream();
			return Task.Run(() =>
			{
				_mainPicture = PictureFactory.Open(_mainPictureStream);
				_mainPicture.Resize(_settings.Columns * _settings.Resolution, _settings.Rows * _settings.Resolution);
			});
		}

		public async Task<RenderResult> RenderAsync(IProgress<MosaicProgress> progress)
		{
			if (_mainPicture == null)
			{
				throw new InvalidOperationException($"Missing main image. call {nameof(SetMainImageAsync)}()");
			}

			var startedAt = DateTime.Now;
			_set.ResetMetadata();

			Grid grid = new Grid(_settings.Columns, _settings.Rows, _settings.UseGridSearch);
			Matcher matcher = new(grid, _set, _mainPicture, _settings);
			Renderer renderer = new Renderer(grid, _settings.Resolution);

			await LoadSamplesAsync(progress);
			await matcher.FindMatchesAsync(progress);
			var render = await renderer.RenderAsync(progress);

			return RenderResult.Build(startedAt, render, _set);
		}

		private Task LoadSamplesAsync(IProgress<MosaicProgress> stepProgress)
		{
			return Task.Run(() =>
			{
				int total = _set.Tiles.Count();
				int current = 0;
				Parallel.ForEach(_set.Tiles, source =>
				{
					source.LoadSamples(_settings.SamplesPerTile, _settings.UseAverageSamples);
					stepProgress?.Report(new MosaicProgress("Indexing", (float)Interlocked.Increment(ref current) / total));
				});
			});
		}

		public void Dispose()
		{
			if (DisposeMainImage)
			{
				_mainPicture.Dispose();
				_mainPictureStream.Dispose();
			}
		}
	}
}
