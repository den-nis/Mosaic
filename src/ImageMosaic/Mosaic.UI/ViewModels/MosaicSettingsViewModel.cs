using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class MosaicSettingsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly string[] _renderProperties = new[]
		{
			nameof(Columns),
			nameof(Rows),
			nameof(Resolution),
			nameof(Contrast),
			nameof(Brightness),
			nameof(Red),
			nameof(Green),
			nameof(Blue),
		};

		private int _columns;
		public int Columns 
		{
			get => _columns;
			set
			{
				_columns = value;
				NotifyPropertyChanged();
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
			}
		}

		private int _contrast;
		public int Contrast 
		{
			get => _contrast;
			set
			{
				_contrast = value;
				NotifyPropertyChanged();
			}
		}

		private int _brightness;
		public int Brightness 
		{
			get => _brightness;
			set
			{
				_brightness = value;
				NotifyPropertyChanged();
			}
		}

		private int _red;
		public int Red 
		{
			get => _red;
			set
			{
				_red = value;
				NotifyPropertyChanged();
			}
		}

		private int _green;
		public int Green 
		{
			get => _green;
			set
			{
				_green = value;
				NotifyPropertyChanged();
			}
		}

		private int _blue;
		public int Blue 
		{
			get => _blue;
			set
			{
				_blue = value;
				NotifyPropertyChanged();
			}
		}

		private string _renderDetails;
		public string RenderDetails
		{
			get => _renderDetails;
			set
			{
				_renderDetails = value;
				NotifyPropertyChanged();
			}
		}

		private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			if (_renderProperties.Contains(propertyName))
				RenderPropertiesChanged();
		}

		private void RenderPropertiesChanged()
		{
			RenderDetails = $"{Rows}x{Columns}";
		}
	}
}
