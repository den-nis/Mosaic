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
	public class MosaicImage
	{
		private readonly RenderSettings _settings;

		public IEnumerable<PictureSource> TilePictures { get; set; }
		public PictureSource MainPicture { get; set; }

		private int _rows;
		private int _columns;

		public MosaicImage(RenderSettings settings)
		{
			_settings = settings;

			if (!_settings.IsValid(out var message))
			{
				throw new ArgumentException(message);
			}
		}

		public RenderResult Render() => Render(null, CancellationToken.None);

		public RenderResult Render(IProgress<MosaicProgress> progress, CancellationToken cancellationToken)
		{
			progress?.Report(new MosaicProgress(MosaicProgress.Steps.Setup, 0));
			var startedAt = DateTime.Now;

			using var main = LoadMainImage();

			using TileSet set = new TileSet(_settings);
			Grid grid = new Grid(_columns, _rows, _settings.UseGridSearch);
			Matcher matcher = new(grid, set, main, _settings);
			Renderer renderer = new Renderer(grid, _settings.Resolution);

			set.LoadTiles(TilePictures, progress, cancellationToken);
			set.IndexSamples(progress, cancellationToken);

			matcher.FindMatches(progress, cancellationToken);
			var render = renderer.Render(progress);

			return RenderResult.Build(startedAt, render, set);
		}

		private IPicture LoadMainImage()
		{
			if (MainPicture == null)
			{
				throw new InvalidOperationException($"Missing main image. assign {nameof(MainPicture)} before calling");
			}

			using var stream = MainPicture.GetDataStream();
			var main = PictureFactory.Open(stream);
			_rows = (int)(main.Height * _settings.Size);
			_columns = (int)(main.Width * _settings.Size);
			main.Resize(_columns * _settings.Resolution, _rows * _settings.Resolution);
			return main;
		}
	}
}
