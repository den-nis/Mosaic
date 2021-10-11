using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	public class PictureSourceFile : PictureSource
	{
		public PictureSourceFile(string path) : base(path)
		{
			
		}

		public override Stream GetDataStream()
		{
			return File.OpenRead(Identifier);
		}
	}
}
