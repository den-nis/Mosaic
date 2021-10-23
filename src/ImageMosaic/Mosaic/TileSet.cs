using Mosaic.Progress;
using Mosaic.Settings;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic
{
	internal class TileSet : IDisposable
	{
		public int Resolution { get; }
		public CropMode CropMode { get; }
		public bool AverageSamples { get; }
		public int SamplesPerTile { get; }

		private bool _loaded = false;

		public HashSet<Tile> Tiles { get; private set; } = new();

		public TileSet(RenderSettings settings)
		{
			Resolution = settings.Resolution;
			CropMode = settings.CropMode;
			AverageSamples = settings.UseAverageSamples;
			SamplesPerTile = settings.SamplesPerTile;
		}

		public void LoadTiles(IEnumerable<PictureSource> sources, IProgress<MosaicProgress> progress, CancellationToken ct)
		{
			Load(sources.Select(s => new Tile(s)), progress, ct);
		}

		public void IndexSamples(IProgress<MosaicProgress> progress, CancellationToken ct)
		{
			if (!_loaded)
			{
				throw new InvalidOperationException("Tiles must be loaded first");
			}

			int total = Tiles.Count;
			int current = 0;
 
			Parallel.ForEach(Tiles, source =>
			{
				source.LoadSamples(SamplesPerTile, AverageSamples);
				progress?.Report(new MosaicProgress(MosaicProgress.Steps.Indexing, (float)Interlocked.Increment(ref current) / total, source.Source.Identifier));
				ct.ThrowIfCancellationRequested();
			});
		}

		private void Load(IEnumerable<Tile> tiles, IProgress<MosaicProgress> progress, CancellationToken ct)
		{
			if (_loaded)
			{
				throw new InvalidOperationException("Tiles are already loaded");
			}

			Tiles = new HashSet<Tile>(tiles);

			int total = Tiles.Count;
			int current = 0;

			Parallel.ForEach(Tiles, tile =>
			{
				progress?.Report(new MosaicProgress(MosaicProgress.Steps.Loading, (float)Interlocked.Increment(ref current) / total, tile.Source.Identifier));
				tile.Load(Resolution, CropMode);
				ct.ThrowIfCancellationRequested();
			});

			_loaded = true;
		}

		public void Dispose()
		{
			foreach(var tile in Tiles)
			{
				tile?.Dispose();
			}

			Tiles.Clear();
		}		
	}
}