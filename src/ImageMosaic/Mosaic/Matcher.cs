﻿using Mosaic.Settings;
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

namespace Mosaic
{
	/// <summary>
	/// Places the pictures on the right spots
	/// </summary>
	internal class Matcher
	{
		private readonly RenderSettings _settings;
		private readonly Grid _grid;
		private readonly TileSet _set;
		private readonly IPicture _main;

		private int _repeatRadius;
		private Tile[] _tileArray;

		public Matcher(Grid grid, TileSet sources, IPicture main, RenderSettings settings)
		{
			_settings = settings;
			_set = sources;
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
		public void FindMatches(IProgress<MosaicProgress> progress, CancellationToken token)
		{
			_repeatRadius = GetRepeatRadius();
			_main.CachePixels();

			try
			{
				_tileArray = _set.Tiles.ToArray();

				for (int y = 0; y < _grid.Height; y++)
				{
					for (int x = 0; x < _grid.Width; x++)
					{
						progress?.Report(new MosaicProgress(MosaicProgress.Steps.Matching, ((x + y * _grid.Width) + 1f) / (_grid.Width * _grid.Height)));
						token.ThrowIfCancellationRequested();

						HandleTile(x, y);
					}
				}
			}
			finally
			{
				_main.UncachePixels();
			}
		}

		private void HandleTile(int x, int y)
		{
			var suggestions = GetOptions(x, y);
			FillDistances(suggestions, x, y);
			PlaceTile(suggestions, x, y);
		}

		private void PlaceTile(PlaceOption[] suggestions, int x, int y)
		{
			var choice = suggestions
				.OrderByDescending(s => s.RepeatDistance)
				.ThenBy(s => s.Difference)
				.First();

			_grid.SetTile(choice, x, y);
			choice.Source.TimesUsed++;
		}

		private void FillDistances(PlaceOption[] suggestions, int x, int y)
		{
			for (int i = 0; i < suggestions.Length; i++)
			{
				float distance = _grid.Nearest(x, y, _repeatRadius, suggestions[i].Source);
				suggestions[i].RepeatDistance = distance;
			}
		}

		private PlaceOption[] GetOptions(int x, int y)
		{
			var placer = PlacerFactory.Create(_settings.UseRotation, _settings.UseMirror);
			var mainSample = SetupMainSample(x, y);
			placer.SetMainSample(mainSample);

			PlaceOption[] buffer = new PlaceOption[_tileArray.Length * placer.Size];
			placer.SetBuffer(buffer);

			Parallel.For(0, _tileArray.Length, i => 
			{
				placer.FillOptions(i * placer.Size, _tileArray[i]);
			});

			return buffer;
		}

		private SampleSet SetupMainSample(int x, int y)
		{
			var samples = _main.ComputeSamples(_settings.SamplesPerTile,
				_settings.UseAverageSamples,
				_set.Resolution,
				x * _set.Resolution,
				y * _set.Resolution
			);

			return new SampleSet(samples, _set.Resolution);
		}

		private int GetRepeatRadius()
		{
			if (_settings.RepeatRadius == -1)
			{
				double maxRadius = Math.Sqrt(
					_grid.Width * _grid.Width +
					_grid.Height * _grid.Height
				);

				int tiles = _set.Tiles.Count;
				int spots = _grid.Width * _grid.Height;

				return (int)Math.Round(Math.Min((float)tiles / spots * 25f, maxRadius));
			}
			else
			{
				return _settings.RepeatRadius;
			}
		}
	}
}
