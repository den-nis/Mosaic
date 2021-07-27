using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public static class MagickColorExtensions
	{
		public static int GetHue(this MagickColor color)
		{
			var r = color.R / (float)short.MaxValue;
			var g = color.G / (float)short.MaxValue;
			var b = color.B / (float)short.MaxValue;

			var max = Math.Max(Math.Max(r, g), b);
			var min = Math.Min(Math.Min(r, g), b);

			float hueValue = 60;

			if (r == max)
			{
				hueValue *= (g - b) / (max - min);
			}
			else if (g == max)
			{
				hueValue *=  2f + (b - r) / (max - min);
			}
			else
			{
				hueValue *= 4f + (r - g) / (max - min);
			}

			if (hueValue < 0)
			{
				hueValue += 360;
			}

			return (int)hueValue;
		}
	}
}
