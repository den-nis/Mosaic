using ImageMagick;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	class TileGridRenderer
	{
		public int ThumbnailResolution { get; set; }
		public int RenderResolution { get; set; }

		public bool TemporaryStorage { get; set; } = true;
		public string TemporaryFolder { get; set; } = "/";

		private readonly TileGrid _grid;

		public TileGridRenderer(TileGrid grid)
		{
			_grid = grid;
		}

		public MagickImage Render(bool useThumbnails, IProgress<MosaicProgress> progress = null)
		{
			MagickImage result = new(MagickColor.FromRgb(0,0,0), 1, 1);
			int resolution = useThumbnails ? ThumbnailResolution : RenderResolution;

			if (TemporaryStorage)
			{
				result.Settings.SetDefine("temporary-path", TemporaryFolder);
			}

			result.Scale(new MagickGeometry
			{
				Width = _grid.Width * resolution,
				Height = _grid.Height * resolution,
				IgnoreAspectRatio = true,
			});

			for (int y = 0; y < _grid.Height; y++)
			{
				progress.Report(new MosaicProgress("Rendering", (y + 1) / ((float)_grid.Height)));
				for (int x = 0; x < _grid.Width; x++)
				{
					var image = useThumbnails ? _grid[x, y].Thumbnail : _grid[x, y].Image;
					result.Composite(image, x * resolution, y * resolution, CompositeOperator.Over);
				}
			}

			return result;
		}
	}
}
