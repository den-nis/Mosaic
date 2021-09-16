using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Progress
{
	public class TileProgress
	{
		public int X { get; set; }
		public int Y { get; set; }

		public TileProgress(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"Placed tile {X}x{Y}";
		}
	}
}
