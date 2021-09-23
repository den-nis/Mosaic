using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public abstract class PictureSource : IEquatable<PictureSource>, IComparable<PictureSource>
	{
		public string Identifier { get; set; }

		public PictureSource(string identifier)
		{
			Identifier = identifier;
		}

		public abstract Stream GetDataStream();

		public bool Equals(PictureSource other)
		{
			return Identifier == other.Identifier;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Tile);
		}

		public override int GetHashCode()
		{
			return Identifier.GetHashCode();
		}

		public int CompareTo(PictureSource other)
		{
			return Identifier.CompareTo(other.Identifier);
		}
	}
}