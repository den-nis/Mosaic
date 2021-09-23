using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mosaic
{
	internal class Tile : IEquatable<Tile>, IComparable<Tile>
	{
		public PictureSource Source { get; set; }

		private IPicture _picture;
		internal IPicture Picture
		{
			get => _picture ?? throw new InvalidOperationException("Image must be loaded first");
			private set => _picture = value;
		}

		public SampleSet SampleSet { get; set; }

		public int TimesUsed { get; set; } = 0; //Metadata

		public Tile(PictureSource source)
		{
			Source = source;
		}

		public void ResetMetadata()
		{
			TimesUsed = 0;
		}

		public void LoadSamples(int amount, bool useAverage)
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

		public void Load(int resolution, CropMode crop)
		{
			var sourceStream = Source.GetDataStream();
			sourceStream.Position = 0;

			IPicture picture = PictureFactory.Open(sourceStream);
			picture.ApplyCropMode(crop); //TODO: fix
			picture.Resize(resolution, resolution);
			Picture = picture;
		}

		public bool Equals(Tile other) => other.Source.Equals(other.Source);
		
		public override bool Equals(object obj) => Equals(obj as Tile);
		
		public override int GetHashCode() => Source.GetHashCode();

		public int CompareTo(Tile other) => Source.CompareTo(other.Source);
	}
}
