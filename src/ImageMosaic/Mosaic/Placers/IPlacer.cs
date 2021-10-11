using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal interface IPlacer
	{
		int Size { get; }

		void SetMainSample(SampleSet main);

		void SetBuffer(PlaceOption[] bufer);

		void FillOptions(int index, Tile source);
	}
}
