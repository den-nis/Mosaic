using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.ImageMatching
{
	interface ITileMatcher
	{
		void SetTiles(IEnumerable<TileImage> tiles);

		TileImage FindMatch(MagickColor color);
	}
}
