using AutoMapper;
using CommandLine;
using Mosaic.Graphics;
using Mosaic.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic.CLI
{ 
	internal class Program
	{
		private static readonly MapperConfiguration _mapperConfig = new(cfg => cfg.CreateMap<Options, RenderSettings>());

		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
		}

		static void Run(Options options)
		{
			ConsoleProgress progress = new();
			Console.WriteLine();

			SearchOption searchOption = options.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			string[] files = Directory.GetFiles(options.Input, options.Filter, searchOption);

			MosaicImage mosaic = new(MapRenderSettings(options))
			{
				MainPicture = new PictureSourceFile(options.Main),
				TilePictures = files.Select(f => new PictureSourceFile(f))
			};

			RenderResult result = mosaic.Render(progress, CancellationToken.None);
			result.Write(options.Output);

			Console.WriteLine();
			Console.WriteLine("Finsihed render. ");
			Console.WriteLine($"Time taken: {result.Time.TotalSeconds} seconds");
			Console.WriteLine($"Pictures used: {result.UsedPictures}/{result.TotalPictures}");
			Console.WriteLine($"Saved to: \"{Path.GetFullPath(options.Output)}\"");
		}

		static RenderSettings MapRenderSettings(Options options)
		{
			var mapper = _mapperConfig.CreateMapper();
			return mapper.Map<RenderSettings>(options);
		}
	}
}
