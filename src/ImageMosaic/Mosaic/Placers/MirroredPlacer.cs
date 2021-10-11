using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal class MirroredPlacer : IPlacer
	{
		public int Size => 2;

		private PlaceOption[] _buffer;
		private SampleSet _mainNormal;
		private SampleSet _mainMirrored;

		public void FillOptions(int index, Tile source)
		{
			_buffer[index] = new PlaceOption(source, 0, false, source.SampleSet.SamplesDifference(_mainNormal));
			_buffer[index + 1] = new PlaceOption(source, 0, true, source.SampleSet.SamplesDifference(_mainMirrored));
		}

		public void SetBuffer(PlaceOption[] buffer)
		{
			_buffer = buffer;
		}

		public void SetMainSample(SampleSet main)
		{
			_mainNormal = main;
			_mainMirrored = main.Copy();
			_mainMirrored.Mirror();
		}
	}
}
