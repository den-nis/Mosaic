using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mosaic
{
	public class TileSource : IEquatable<TileSource>, IComparable<TileSource>
	{
		public string FullPath { get; set; }
		public CropMode CropMode { get; set; }

		private IPicture _picture;
		internal IPicture Picture
		{
			get => _picture ?? throw new InvalidOperationException("Image must be loaded first");
			private set => _picture = value;
		}

		internal SampleSet SampleSet { get; set; }

		//Metadata
		internal int TimesUsed { get; set; } = 0;

		public TileSource(string path, CropMode cropMode)
		{
			FullPath = path;
			CropMode = cropMode;
		}

		internal void ResetMetadata()
		{
			TimesUsed = 0;
		}

		internal void LoadSamples(int amount, bool useAverage)
		{
			if (SampleSet == null || SampleSet.RegionSize != Picture.Width || SampleSet.Amount != amount)
			{
				RefreshSamples(amount, useAverage);
			}
		}

		private void RefreshSamples(int amount, bool useAverage)
		{
			SampleSet = new SampleSet(Picture.ComputeSamples(amount, useAverage), Picture.Width);
		}

		internal void Load(int resolution)
		{
			IPicture picture = PictureFactory.Open(FullPath);
			picture.ApplyCropMode(CropMode);
			picture.Resize(resolution, resolution);
			Picture = picture;
		}

		public bool Equals(TileSource other)
		{
			return FullPath == other.FullPath && CropMode == other.CropMode;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as TileSource);
		}

		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}

		public int CompareTo(TileSource other)
		{
			int compare = FullPath.CompareTo(other.FullPath);
			if (compare == 0)
				return CropMode.CompareTo(other.CropMode);

			return compare;
		}
	}
}
