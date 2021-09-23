using ImageMagick;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaic.Graphics;
using Mosaic.Placers;

namespace Mosaic
{
    class Grid
    {
        public TileUsage this[int x, int y] => _grid[x, y];

        public int Width => _grid.GetLength(0);
        public int Height => _grid.GetLength(1);

        private readonly bool _useGridSearch;

        private TileUsage[,] _grid;
        private Dictionary<Tile, List<TileUsage>> _byTileSource = new();

        public Grid(int width, int height, bool useGridSearch)
        {
            _useGridSearch = useGridSearch;
            _grid = new TileUsage[width, height];
        }

        private void AddToLookup(TileUsage usage)
        {
            if (_useGridSearch) //Storing data in lookup not needed when using grid search
            {
                return;
            }

            if (_byTileSource.TryGetValue(usage.Source, out List<TileUsage> items))
            {
                items.Add(usage);
            }
            else
            {
                items = new List<TileUsage>() { usage };
                _byTileSource.Add(usage.Source, items);
            }
        }

        public void SetTile(PlaceOption option, int x, int y)
        {
            _grid[x, y] = new TileUsage(option, x, y);
            AddToLookup(_grid[x, y]);
        }

        public float Nearest(int x, int y, int radius, Tile source)
        {
            float nearest = float.MaxValue;

            if (_useGridSearch)
            {
                nearest = FindNearestGrid(x, y, radius, source);
            }
            else
            {
                if (_byTileSource.TryGetValue(source, out List<TileUsage> tiles))
                {
                    nearest = FindNearest(x, y, radius, tiles);
                }
            }

            if (nearest > radius)
            {
                nearest = float.MaxValue;
            }

            return nearest;
        }

        private float FindNearest(int x, int y, int radius, List<TileUsage> tiles)
        {
            if (radius == 0)
                return float.MaxValue;

            float nearest = float.MaxValue;
            foreach (var tile in tiles)
            {
				int deltaX = Math.Abs(tile.X - x);
                int deltaY = Math.Abs(tile.Y - y);

                if (deltaX <= radius && deltaY <= radius)
                {
                    var distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    nearest = Math.Min(nearest, distance);
                }
            }

            return nearest;
        }

        /// <summary>
        /// Ignores everything greater than "y".
        /// Should be used for finding duplicates when filling the grid linearly top to bottom
        /// </summary>
        private float FindNearestGrid(int x, int y, int radius, Tile source)
        {
            if (radius == 0)
                return float.MaxValue;

            int left   = Clip(x - radius, Width, 0);
            int right  = Clip(x + radius, Width, 0);
            int top    = Clip(y - radius, Height, 0);
            int bottom = Clip(y + 1, Height, 0);

            float nearest = float.MaxValue;

            for (int iy = top; iy < bottom; iy++)
            {
                for (int ix = left; ix < right; ix++)
                {
                    if (_grid[ix, iy] != null && (ix != x || iy != y) && _grid[ix, iy].Source.Equals(source))
                    {
                        int deltaX = Math.Abs(ix - x);
                        int deltaY = Math.Abs(iy - y);
                        float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                        nearest = Math.Min(nearest, distance);
                    }
                }
            }
            
            return nearest;
        }

        private int Clip(int value, int max, int min)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
