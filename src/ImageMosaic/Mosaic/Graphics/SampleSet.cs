using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics
{
	internal class SampleSet : IEquatable<SampleSet>
	{
		public int RegionSize { get; }
		private Sample[] _samples { get; set; }
		public int Amount => _samples.Length;

		public SampleSet(Sample[] samples, int regionSize)
		{
			_samples = samples;
			RegionSize = regionSize;
			Order();
		}

		public float SamplesDifference(SampleSet other)
		{
			float difference = 0;
			for (int i = 0; i < other._samples.Length; i++)
			{
				difference += Color.ColorDistance(other._samples[i].Color, _samples[i].Color);
			}

			return difference;
		}

		public void Mirror()
		{
			if (_samples.Length == 1)
				return;

			int center = RegionSize / 2;

			for (int i = 0; i < _samples.Length; i++)
			{
				int x = _samples[i].X - center;
				int y = _samples[i].Y - center;

				_samples[i].X = x * -1 + center;
				_samples[i].Y = y + center;
			}

			Order();
		}

		public void Rotate(int angle)
		{
			if (_samples.Length == 1)
				return; 

			int center = RegionSize / 2;

			for (int i = 0; i < _samples.Length; i++)
			{
				for (int r = 0; r < angle; r++)
				{
					int x = _samples[i].X - center;
					int y = _samples[i].Y - center;

					_samples[i].X = -y + center;
					_samples[i].Y = x + center;
				}
			}

			Order();
		}

		private void Order()
		{
			_samples = _samples.OrderBy(s => s.Y).ThenBy(s => s.X).ToArray();
		}

		public SampleSet Copy()
		{
			return new SampleSet(_samples, RegionSize);
		}

		public bool Equals(SampleSet other)
		{
			return RegionSize == other.RegionSize && _samples.SequenceEqual(other._samples);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as SampleSet);
		}

		public override int GetHashCode()
		{
			return _samples[0].Color.Red.GetHashCode() + _samples[0].Color.Red + _samples[0].Color.Blue;
		}
	}
}
