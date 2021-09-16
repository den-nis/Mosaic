using Mosaic.Placers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	/// <summary>
	/// Represents a tile placed in the grid
	/// </summary>
	record TileUsage : IEquatable<TileUsage>
	{
		public TileSource Source { get; init; }
		public int Rotation { get; init; }
		public bool Mirrored { get; init; }
		public int X { get; init; }
		public int Y { get; init; }

		public TileUsage(PlaceOption option, int x, int y)
		{
			Source = option.Source;
			Rotation = option.Rotation;
			Mirrored = option.Mirrored;
			X = x;
			Y = y;
		}
	}
}
