using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal class RotationPlacer : IPlacer
	{
		public int Size => 4;

		private PlaceOption[] _buffer;
		private SampleSet[] _main;

		public void FillOptions(int index, Tile source)
		{
			for (int i = 0; i < _main.Length; i++)
			{
				var difference = source.SampleSet.SamplesDifference(_main[i]);
				_buffer[index + i] = new PlaceOption(source, 0 - i, false, difference);
			}
		}

		public void SetBuffer(PlaceOption[] buffer)
		{
			_buffer = buffer;
		}

		public void SetMainSample(SampleSet main)
		{
			_main = new SampleSet[4];
			_main[0] = main;
			for (int i = 1; i < _main.Length; i++)
			{
				_main[i] = main.Copy();
				_main[i].Rotate(i);
			}
		}
	}
}
