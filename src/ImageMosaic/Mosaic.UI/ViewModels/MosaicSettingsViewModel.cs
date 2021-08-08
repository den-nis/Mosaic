using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class MosaicSettingsViewModel : INotifyPropertyChanged
	{ 
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

		private readonly ObservableCollection<ImageViewModel> _images = new();
		public ObservableCollection<ImageViewModel> Images => _images;
		public event PropertyChangedEventHandler PropertyChanged;

		public MosaicSettingsViewModel()
		{
			Columns = 16;
			Rows = 16;
			Resolution = 32;
		}

		public void OpenImages(IEnumerable<string> files)
		{
			var imageModels = files.Select(f => new ImageViewModel
			{
				Fullpath = f,
				IsMainImage = false
			});

			foreach(var image in imageModels)
			{
				if (!Images.Contains(image))
				{
					Images.Add(image);
				}
			}

			TrySetDefaultMain();
			StartRender();
		}

		public void RemoveImage(ImageViewModel viewModel)
		{
			Images.Remove(viewModel);
			TrySetDefaultMain();
		}

		private void TrySetDefaultMain()
		{
			if (!HasMainImage() && Images.Any())
			{
				SetMainImage(Images.First());
			}
		}

		private bool HasMainImage() => Images.Any(i => i.IsMainImage);
		
		public void SetMainImage(ImageViewModel viewModel)
		{
			foreach(var image in Images)
			{
				image.IsMainImage = false;

				if (image.Fullpath == viewModel.Fullpath)
				{
					image.IsMainImage = true;
				}
			}

			StartRender();
		}

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
			RenderDetails = BuildDetailString();
			StartRender();
		}

		private string BuildDetailString()
		{
			GetAspectRatio(Rows, Columns, out int x, out int y);

			return string.Join(Environment.NewLine,
				$"",
				$"Cell resolution : {Resolution}",
				$"Final image size : {Columns * Resolution}x{Rows * Resolution}",
				$"Final image aspect ratio : {x}:{y}"
			
			);

		}

		private static void GetAspectRatio(int width, int height, out int x, out int y)
		{
			if (width == 0 && height == 0)
			{
				x = y = 0;
				return;
			}

			for (int primary = 1; ;primary++)
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

		private void StartRender()
		{
			throw new NotImplementedException();
		}
	}
}
