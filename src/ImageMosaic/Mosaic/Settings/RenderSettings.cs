using System;

namespace Mosaic.Settings
{
	public record RenderSettings
	{
		public int Columns { get; init; }
		public int Rows { get; init; }
		public int Resolution { get; init; }

		public bool UseRotation { get; init; }
		public bool UseMirror { get; init; }

		public int SamplesPerTile { get; init; }

		/// <summary>
		/// Instead of one pixel it will be the average of the pixels around it.
		/// Is a lot slower but will give better results
		/// </summary>
		public bool UseAverageSamples { get; init; }

		/// <summary>
		/// Within this radius pictures should not repeat but might do if there is no alternative tile
		/// </summary>
		public int RepeatRadius { get; init; }

		/// <summary>
		/// Using the grid instead of a lookup for searching repeating tiles.
		/// Might speed up a render with lots of duplicates and a small but non zero repeat radius
		/// </summary>
		public bool UseGridSearch { get; init; }

		public bool IsValid(out string message)
		{
			if (Math.Sqrt(SamplesPerTile) > Resolution)
			{
				message = "Too many samples for current resolution";
				return false;
			}

			message = string.Empty;
			return true;
		}
	}
}
