using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Mosaic.GUI.Models
{
	public class PictureModel : IEquatable<PictureModel>, IComparable<PictureModel>
	{
		public bool IsMain { get; set; }
		public string FullPath { get; set; }

		public int Width { get; private set; }
		public int Height { get; private set; }
		private bool _isMetaLoaded;

		public PictureModel(string fullPath)
		{
			FullPath = fullPath;
		}

		/// <summary>
		/// Returns false if the image is invalid
		/// </summary>
		public bool LoadMeta()
		{
			if (!_isMetaLoaded)
			{
				using FileStream imageStream = File.OpenRead(FullPath);
				BitmapDecoder decoder;
				try
				{
					decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
				}
				catch(NotSupportedException)
				{
					return false;
				}

				Width = decoder.Frames[0].PixelHeight;
				Height = decoder.Frames[0].PixelWidth;
				_isMetaLoaded = true;
			}

			return true;
		}

		public bool Equals(PictureModel other)
		{
			return FullPath == other.FullPath;
		}

		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}

		public int CompareTo(PictureModel other) => FullPath.CompareTo(other.FullPath);
	}
}