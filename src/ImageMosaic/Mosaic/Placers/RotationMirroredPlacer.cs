using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Placers
{
	internal class RotationMirroredPlacer : IPlacer
	{
		public int Size => 8;

		private PlaceOption[] _buffer;
		private SampleSet[] _main;

		public void FillOptions(int index, TileSource source)
		{
			for (int i = 0; i < _main.Length; i++)
			{
				int rotation = i / 2;
				bool mirror = i % 2 > 0;

				var difference = source.SampleSet.SamplesDifference(_main[i]);
				_buffer[index + i] = new PlaceOption(source, 0 - rotation, mirror, difference);
			}
		}

		public void SetBuffer(PlaceOption[] buffer)
		{
			_buffer = buffer;
		}

		public void SetMainSample(SampleSet main)
		{
			_main = new SampleSet[8];
			_main[0] = main;
			_main[1] = main.Copy();
			_main[1].Mirror();

			for (int i = 2; i < _main.Length; i += 2)
			{
				_main[i] = main.Copy();
				_main[i + 1] = main.Copy();

				_main[i].Rotate(i / 2);
				_main[i + 1].Rotate(i / 2);
				_main[i + 1].Mirror();
			}
		}
	}
}
