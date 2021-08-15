using ImageMagick;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	class TileGridRenderer
	{
		private readonly int _resolution;
		private readonly TileGrid _grid;

		public TileGridRenderer(TileGrid grid, int resolution)
		{
			_grid = grid;
			_resolution = resolution;
		}

		public Task<MagickImage> RenderAsync(IProgress<MosaicProgress> progress)
		{
			return Task.Run(() => Render(progress));
		}

		private MagickImage Render(IProgress<MosaicProgress> progress)
		{
			progress?.Report(new MosaicProgress("Preparing render"));
			MagickImage result = new(MagickColor.FromRgb(0, 0, 0), 1, 1);

			result.Scale(new MagickGeometry
			{
				Width = _grid.Width * _resolution,
				Height = _grid.Height * _resolution,
				IgnoreAspectRatio = true,
			});

			for (int y = 0; y < _grid.Height; y++)
			{
				progress?.Report(new MosaicProgress("Rendering", (y + 1) / ((float)_grid.Height)));
				for (int x = 0; x < _grid.Width; x++)
				{
					var image = _grid[x, y].Image;
					result.Composite(image, x * _resolution, y * _resolution, CompositeOperator.Over);
				}
			}

			return result;
		}
	}
}
