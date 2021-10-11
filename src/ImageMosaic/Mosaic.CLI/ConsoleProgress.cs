using Mosaic.Progress;
using System;

namespace Mosaic.CLI
{
	internal class ConsoleProgress : IProgress<MosaicProgress>
	{
		private int _lastBufferSize = 0;

		public void Report(MosaicProgress value)
		{
			WriteBuffer($"{value.StepDisplayName()}... {Math.Round(value.Percentage * 100, 2)}% {value.AdditionalInfo}");
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