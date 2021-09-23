using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal struct PlaceOption
	{
		public Tile Source { get; set; }
		public int Rotation { get; set; }
		public bool Mirrored { get; set; }

		public float Difference { get; set; }
		public float RepeatDistance { get; set; }

		public PlaceOption(Tile source, int rotation, bool isMirrored, float difference)
		{
			Source = source;
			Rotation = rotation;
			Mirrored = isMirrored;
			Difference = difference;
			RepeatDistance = 0;
		}
	}
}
