using AutoMapper;
using CommandLine;
using Mosaic.Graphics;
using Mosaic.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.CLI
{ 
	class Program
	{
		private static readonly MapperConfiguration _mapperConfig = new(cfg => cfg.CreateMap<Options, RenderSettings>());

		static async Task Main(string[] args)
		{
			await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(Run);
		}

		static async Task Run(Options options)
		{
			var progress = new ConsoleProgress();
			Console.WriteLine();

			var searchOption = options.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			var files = Directory.GetFiles(options.Input, options.Filter, searchOption);

			using TileSet set = new TileSet(options.Resolution, options.Crop);
			await set.LoadTilesAsync(files.Select(f => new PictureSourceFile(f)), progress);

			MosaicImage mosaic = new MosaicImage(set, MapRenderSettings(options));
			await mosaic.SetMainImageAsync(new PictureSourceFile(options.Main));
			var result = await mosaic.RenderAsync(progress);

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
