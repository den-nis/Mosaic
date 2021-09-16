using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal static class PlacerFactory
	{
		public static IPlacer Create(bool useRotation, bool useMirror)
		{
			switch((useRotation, useMirror))
			{
				case (true, false): return new RotationPlacer();
				case (false, true): return new MirroredPlacer();

				case (true, true): return new RotationMirroredPlacer();
				case (false, false): return new SimplePlacer();

				default:
					throw new KeyNotFoundException("No placer found for this configuration");
			}
		}
	}
}
