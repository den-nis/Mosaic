using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class RenderSettingsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public int _columns;
		public int Columns 
		{
			get => _columns;
			set
			{
				_columns = value;
				NotifyPropertyChanged();
			}
		}

		public int _rows;
		public int Rows
		{
			get => _rows;
			set
			{
				_rows = value;
				NotifyPropertyChanged();
			}
		}

		public int _resolution;
		public int Resolution 
		{
			get => _resolution;
			set
			{
				_resolution = value;
				NotifyPropertyChanged();
			}
		}

		public int _contrast;
		public int Contrast 
		{
			get => _contrast;
			set
			{
				_contrast = value;
				NotifyPropertyChanged();
			}
		}

		public int _brightness;
		public int Brightness 
		{
			get => _brightness;
			set
			{
				_brightness = value;
				NotifyPropertyChanged();
			}
		}

		public int _red;
		public int Red 
		{
			get => _red;
			set
			{
				_red = value;
				NotifyPropertyChanged();
			}
		}

		public int _green;
		public int Green 
		{
			get => _green;
			set
			{
				_green = value;
				NotifyPropertyChanged();
			}
		}

		public int _blue;
		public int Blue 
		{
			get => _blue;
			set
			{
				_blue = value;
				NotifyPropertyChanged();
			}
		}

		private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
