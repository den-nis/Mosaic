using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic
{
	class PictureSourceStream : PictureSource, IDisposable
	{
		private readonly Stream _stream;

		public PictureSourceStream(Stream stream) : base(Guid.NewGuid().ToString())
		{
			_stream = stream;
		}

		public PictureSourceStream(Stream stream, string identifier) : base(identifier)
		{
			_stream = stream;
		}

		public override Stream GetDataStream()
		{
			return _stream;
		}

		public void Dispose()
		{
			_stream.Dispose();
		}
	}
}
