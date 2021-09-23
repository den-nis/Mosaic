using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal class SimplePlacer : IPlacer
	{
		public int Size => 1;

		private PlaceOption[] _buffer;
		private SampleSet _main;

		public void FillOptions(int index, Tile source)
		{
			_buffer[index] = new PlaceOption(source, 0, false, source.SampleSet.SamplesDifference(_main));
		}

		public void SetBuffer(PlaceOption[] buffer)
		{
			_buffer = buffer;
		}

		public void SetMainSample(SampleSet main)
		{
			_main = main;
		}
	}
}
