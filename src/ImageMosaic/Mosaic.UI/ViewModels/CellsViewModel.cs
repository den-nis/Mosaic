using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class CellsViewModel : ViewModelBase
	{
		public CellsViewModel()
		{
			Columns = 32;
			Rows = 32;
			Resolution = 32;
		}

		private int _columns;
		public int Columns
		{
			get => _columns;
			set
			{
				_columns = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Info));
			}
		}

		private int _rows;
		public int Rows
		{
			get => _rows;
			set
			{
				_rows = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Info));
			}
		}

		private int _resolution;
		public int Resolution
		{
			get => _resolution;
			set
			{
				_resolution = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Info));
			}
		}

		public string Info => GetInfoString();

		private string GetInfoString()
		{
			GetAspectRatio(Columns, Rows, out int x, out int y);

			return string.Join(Environment.NewLine,
					$"Cells: {Columns}x{Rows}",
					$"Pixels: {Columns * Resolution}x{Rows * Resolution} ",
					$"Pictures: {Columns * Rows}",
					$"Ratio: {x}:{y}"
				);
		}

		private static void GetAspectRatio(int width, int height, out int x, out int y)
		{
			if (width == 0 && height == 0)
			{
				x = y = 0;
				return;
			}

			for (int primary = 1; ; primary++)
			{
				var secondary = primary * (height / (double)width);

				if (secondary % 1 == 0)
				{
					x = primary;
					y = (int)secondary;
					return;
				}
			}
		}
	}
}
