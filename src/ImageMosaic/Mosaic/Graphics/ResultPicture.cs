using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics
{
	//Public wrapper class for pictures

	public class ResultPicture : IDisposable
	{
		internal IPicture InternalPicture { get; private set; }

		internal ResultPicture(IPicture interalPicture)
		{
			InternalPicture = interalPicture;
		}

		public void WriteToFile(string filename)
		{
			InternalPicture.Write(filename);
		}

		public void WriteToStream(Stream stream)
		{
			InternalPicture.WriteToStream(stream);
		}

		public void Dispose()
		{
			InternalPicture.Dispose();
		}
	}
}
