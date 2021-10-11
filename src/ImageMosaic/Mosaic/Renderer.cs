using ImageMagick;
using Mosaic.Graphics;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	internal class Renderer
	{
		private readonly int _resolution;
		private readonly Grid _grid;

		public Renderer(Grid grid, int resolution)
		{
			_grid = grid;
			_resolution = resolution;
		}

		public IPicture Render(IProgress<MosaicProgress> progress)
		{
			IPicture result = PictureFactory.Create(_grid.Width * _resolution, _grid.Height * _resolution);

			for (int y = 0; y < _grid.Height; y++)
			{
				progress?.Report(new MosaicProgress(MosaicProgress.Steps.Rendering, (y + 1f) / _grid.Height));
				for (int x = 0; x < _grid.Width; x++)
				{
					bool altered = _grid[x, y].Mirrored || _grid[x, y].Rotation != 0;

					if (altered)
					{
						using var copy = _grid[x, y].Source.Picture.Copy();

						if (_grid[x, y].Mirrored)
						{
							copy.Mirror();
						}
						copy.Rotate(_grid[x, y].Rotation * 90);
						result.Composite(copy, x * _resolution, y * _resolution);
					}
					else
					{
						result.Composite(_grid[x, y].Source.Picture, x * _resolution, y * _resolution);
					}
				}
			}

			return result;
		}
	}
}
