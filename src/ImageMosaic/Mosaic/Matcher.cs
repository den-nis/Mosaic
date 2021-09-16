using Mosaic.Settings;
using Mosaic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using Mosaic.Placers;
using ImageMagick;
using Mosaic.Progress;
using static Mosaic.Progress.TileProgress;

namespace Mosaic
{
	/// <summary>
	/// Places the pictures on the right spots
	/// </summary>
	class Matcher
	{
		public IProgress<TileProgress> TileProgress { get; set; }

		private readonly RenderSettings _settings;
		private readonly Grid _grid;
		private readonly TileSources _sources;
		private readonly IPicture _main;

		private TileSource[] _sourcesArray;
		private Dictionary<(int x, int y), List<PlaceOption>> _targets = new();

		public Matcher(Grid grid, TileSources sources, IPicture main, RenderSettings settings)
		{
			_settings = settings;
			_sources = sources;
			_grid = grid;
			_main = main;

			int expectedWidth = sources.Resolution * grid.Width;
			int expectedHeight = sources.Resolution * grid.Height;

			if (main.Width != expectedWidth || main.Height != expectedHeight)
			{
				throw new InvalidOperationException($"Expected main image with size {expectedWidth}x{expectedHeight} but got {main.Width}x{main.Height}");
			}
		}

		/// <summary>
		/// Find matches and places them on the grid
		/// </summary>
		public Task FindMatchesAsync()
		{
			return Task.Run(() =>
			{
				_main.CachePixels();
				_sourcesArray = _sources.ToArray();

				for (int y = 0; y < _grid.Height; y++)
				{
					for (int x = 0; x < _grid.Width; x++)
					{
						HandleTile(x, y);
					}
				}

				_main.UncachePixels();
			});
		}

		private void HandleTile(int x, int y)
		{
			var suggestions = GetOptions(x, y);
			FillDistances(suggestions, x, y);
			PlaceTile(suggestions, x, y);

			TileProgress?.Report(new TileProgress(x, y));
		}

		private void PlaceTile(PlaceOption[] suggestions, int x, int y)
		{
			var choice = suggestions
				.OrderByDescending(s => s.RepeatDistance)
				.ThenBy(s => s.Difference)
				.First();

			_grid.SetTile(choice, x, y);
		}

		private void FillDistances(PlaceOption[] suggestions, int x, int y)
		{
			for (int i = 0; i < suggestions.Length; i++)
			{
				float distance = _grid.Nearest(x, y, _settings.RepeatRadius, suggestions[i].Source);
				suggestions[i].RepeatDistance = distance;
			}
		}

		private PlaceOption[] GetOptions(int x, int y)
		{
			var placer = PlacerFactory.Create(_settings.UseRotation, _settings.UseMirror);
			var mainSample = SetupMainSample(x, y);
			placer.SetMainSample(mainSample);

			PlaceOption[] buffer = new PlaceOption[_sourcesArray.Length * placer.Size];
			placer.SetBuffer(buffer);

			Parallel.For(0, _sourcesArray.Length, i => 
			{
				placer.FillOptions(i * placer.Size, _sourcesArray[i]);
			});

			return buffer;
		}

		private SampleSet SetupMainSample(int x, int y)
		{
			var samples = _main.ComputeSamples(_settings.SamplesPerTile,
				_settings.UseAverageSamples,
				_sources.Resolution,
				x * _sources.Resolution,
				y * _sources.Resolution
			);

			return new SampleSet(samples, _sources.Resolution);
		}
	}
}
