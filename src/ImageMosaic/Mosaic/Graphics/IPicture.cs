using ImageMagick;
using Mosaic.Graphics.Backend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Graphics
{
	/// <summary>
	/// Class that represents an image
	/// </summary>
	internal interface IPicture : IDisposable
	{
		int Width { get; }
		int Height { get; }

		void WriteToStream(Stream stream);

		void Write(string file);

		void Rotate(int angle);

		void Composite(IPicture img, int x, int y);

		Sample[] ComputeSamples(int amount, bool useAverageSamples, int size, int offsetX, int offsetY);

		Sample[] ComputeSamples(int amount, bool useAverageSamples);

		void Mirror();

		IPicture Copy();

		void CachePixels();

		void UncachePixels();

		void Resize(int width, int height);

		void ApplyCropMode(CropMode cropMode);
	}
}
