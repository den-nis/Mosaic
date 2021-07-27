using ImageMagick;
using Mosaic.ImageMatching;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	public class MosaicImage
	{
		public int _thumbnailResolution = 16;
		public int ThumbnailResolution
		{
			get => _thumbnailResolution;
			set
			{
				if (_renderResolution != value)
				{
					_thumbnailResolution = value;
					_mustCallInitialize = true;
				}
			}
		}
		public int _renderResolution = 64;
		public int RenderResolution
		{
			get => _renderResolution;
			set
			{
				if (_renderResolution != value)
				{
					_renderResolution = value;
					_mustCallInitialize = true;
				}
			}
		}

		public float OverlayOpacity { get; set; } = 0f;
		public CompositeOperator OverlayOperator { get; set; } = CompositeOperator.Over;
		public TileImageCollection Tiles { get; }

		private readonly TileGridRenderer _renderer;
		private readonly TileGrid _tileGrid;
		private bool _mustCallInitialize = true;
		private MagickImage _mainImage;

		public MosaicImage()
		{
			Tiles = new TileImageCollection();
			_tileGrid = new TileGrid(Tiles);
			_renderer = new TileGridRenderer(_tileGrid);
		}

		public void SetTileMatcher(ITileMatcher matcher)
		{
			_tileGrid.TileMatcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
		}

		public void SetMainImage(MagickImage image)
		{
			_tileGrid.SetMainImage(image);
			_mainImage = image;
		}

		public MagickImage RenderPreview(IProgress<MosaicProgress> progress = null) => InternalRender(progress, true);

		public MagickImage Render(IProgress<MosaicProgress> progress = null) => InternalRender(progress, false);

		private MagickImage InternalRender(IProgress<MosaicProgress> progress, bool isPreview)
		{
			if (!Tiles.Any())
			{
				throw new InvalidOperationException("No tile images available");
			}

			progress.Report(new MosaicProgress("Initializing"));
			if (_mustCallInitialize)
			{
				Initialize();
				_mustCallInitialize = false;
			}

			progress.Report(new MosaicProgress("Finding image positions"));
			_tileGrid.DetermineLocations();

			progress.Report(new MosaicProgress("Writing to disk"));
			var render = _renderer.Render(isPreview, progress);

			ApplyOverlay(render);
			return render;
		}

		private void ApplyOverlay(MagickImage image)
		{
			if (OverlayOpacity == 0f)
				return;

			using var overlay = new MagickImage(_mainImage);
			overlay.HasAlpha = true;

			overlay.Resize(new MagickGeometry
			{
				Width = image.Width,
				Height = image.Height,
				IgnoreAspectRatio = true,
			});

			overlay.Evaluate(Channels.Alpha, EvaluateOperator.Multiply, OverlayOpacity);
			image.Composite(overlay, OverlayOperator, Channels.All | Channels.Alpha);
		}

		public void Initialize()
		{
			_renderer.RenderResolution = RenderResolution;
			_renderer.ThumbnailResolution = ThumbnailResolution;
			InitializeTiles();
		}

		/// <summary>
		/// (re)initialize all the tiles
		/// </summary>
		private void InitializeTiles()
		{
			foreach (var tile in Tiles)
			{
				tile.Initialize(RenderResolution, ThumbnailResolution);
			}
		}
	}
}
