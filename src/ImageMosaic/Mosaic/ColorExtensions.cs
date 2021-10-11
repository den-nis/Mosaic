using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	internal struct Color
	{
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }

		public Color(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		public static Color FromByteArray(byte[] bytes)
		{
			return new Color
			{
				Red = bytes[0],
				Green = bytes[1],
				Blue = bytes[2],
			};
		}

		public static float ColorDistance(Color x, Color y)
		{
			long rmean = (x.Red + y.Red) / 2;
			int r = x.Red - y.Red;
			int g = x.Green - y.Green;
			int b = x.Blue - y.Blue;
			return (float)Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
		}
	}
}
