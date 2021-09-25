using Mosaic.Progress;
using System;

namespace Mosaic.CLI
{
	internal class ConsoleProgress : IProgress<MosaicProgress>, IProgress<FileProgress>
	{
		private int _lastBufferSize = 0;

		public void Report(MosaicProgress value)
		{
			WriteBuffer($"{value.Name}... {Math.Round(value.Percentage*100, 2)}%");
		}

		public void Report(FileProgress value)
		{
			WriteBuffer($"Processing file {value.Image}");
		}

		public void WriteBuffer(string str)
		{
			int currentBufferSize = str.Length;
			var padding = new string(' ', Math.Max(_lastBufferSize - currentBufferSize, 0));
			_lastBufferSize = currentBufferSize;

			Console.Write($"{str}{padding}\r");
		}
	}
}
