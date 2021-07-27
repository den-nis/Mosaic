using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Progress
{
	public record MosaicProgress
	{
		public MosaicProgress(string name, float percentage)
		{
			Name = name;
			Percentage = percentage;
		}

		public MosaicProgress(string name)
		{
			Name = name;
			Percentage = -1f;
		}

		public string Name { get; set; }
		public float Percentage { get; set; }

		public override string ToString()
		{
			if (Percentage != -1)
			{
				return $"{Name} - {Math.Round(Percentage * 100, 2)}%";
			}
			else
			{
				return $"{Name}...";
			}
		}
	}
}

