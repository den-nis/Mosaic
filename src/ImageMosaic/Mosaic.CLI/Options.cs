using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CLI
{
	internal class Options
	{
		[Option('o', "output",  Default = "result.png", HelpText = "Output path")]
		public string Output { get; set; }

		[Option('i', "input", Required = true, HelpText = "Path for input images")]
		public string Input { get; set; }

		[Option('m', "main", Required = true, HelpText = "Main image")]
		public string Main { get; set; }

		[Option('a', "all", Default = true, HelpText = "Enable subdirectory search")]
		public bool AllDirectories { get; set; }

		[Option('f', "filter", Default = "*.*", HelpText = "The search string to match against the names of files in the input path")]
		public string Filter { get; set; }

		[Option('s', "size", Default = 1, HelpText = "Multiplier for the grid size. 0.5 = 2 tile per pixel")]
		public float Size { get; set; }

		[Option('R', "rotate",  Default = false, HelpText = "Enable tile rotation")]
		public bool UseRotation { get; set; }

		[Option('M', "mirror", Default = false, HelpText = "Enable tile mirroring")]
		public bool UseMirror { get; set; }

		[Option('S', "samples", Default = 4, HelpText = "Amount of color samples per tile")]
		public int SamplesPerTile { get; set; }

		[Option("resolution", Default = 45, HelpText = "The width and height of the tiles")]
		public int Resolution { get; set; }

		[Option("useAverageSamples", Default = true, HelpText = "Take the average of the area around the sample")]
		public bool UseAverageSamples { get; set; }

		[Option("repeatRadius", Default = -1, HelpText = "The target minimum distance between 2 repeating tiles (-1 for auto)")]
		public int RepeatRadius { get; set; }

		[Option("useGridSearch", Default = false, HelpText = "Use the grid instead of the lookup when searching for repeating tiles")]
		public bool UseGridSearch { get; set; }

		[Option("cropMode", Default = CropMode.Center, HelpText = "Method for cropping the tile images if they are not square. (Center, Stretch)")]
		public CropMode CropMode { get; set; }
	}
}
