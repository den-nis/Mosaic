using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics
{
	internal struct Sample
	{
		public Color Color { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public Sample(int x, int y, Color color)
		{
			X = x;
			Y = y;
			Color = color;
		}
	}
}
