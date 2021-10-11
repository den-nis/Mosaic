using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Progress
{
	public record MosaicProgress
	{
		public enum Steps
		{
			Setup,
			Loading,
			Indexing,
			Matching,
			Rendering,
		}

		public Steps Step { get; set; }
		public float Percentage { get; set; }
		public string AdditionalInfo { get; set; }

		public MosaicProgress(Steps step, float percentage) : this(step, percentage, string.Empty) { }

		public MosaicProgress(Steps step) : this(step, -1, string.Empty) { }

		public MosaicProgress(Steps step, float percentage, string additionalInfo)
		{
			Step = step;
			Percentage = percentage;
			AdditionalInfo = additionalInfo;
		}

		public string StepDisplayName()
		{
			return Step switch
			{
				Steps.Setup => "Setup",
				Steps.Loading => "Caching",
				Steps.Indexing => "Indexing",
				Steps.Matching => "Finding matches",
				Steps.Rendering => "Rendering",
				_ => throw new NotImplementedException(),
			};
		}
	}
}

