using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
    public class EffectsViewModel : ViewModelBase
    {
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
	}
}
