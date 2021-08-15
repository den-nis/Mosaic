using Microsoft.Win32;
using Mosaic.Progress;
using Mosaic.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mosaic.UI
{
	public partial class MainWindow : Window, IDisposable
	{
		public MainViewModel ViewModel { get; set; }
		private Stream _currentRender;

		public MainWindow()
		{
			InitializeComponent();

			ViewModel = new MainViewModel(
				ImagesControl.ViewModel,
				EffectsControl.ViewModel,
				CellsControl.ViewModel
				);

			ViewModel.OnPreviewReady += ViewModel_OnPreviewReady;
			ViewModel.OnProgressChanged += ViewModel_OnProgressChanged;

			DataContext = ViewModel;
		}

		private void ViewModel_OnProgressChanged(MosaicProgress progress)
		{
			LabelStatusText.Content = progress.Name;
			ProgressBarPreview.Value = progress.Percentage * 100;
		}

		private void ViewModel_OnPreviewReady(BitmapFrame image, Stream stream)
		{
			Dispatcher.Invoke(() => {
				ImagePreview.Source = image;
				_currentRender = stream;
			});
		}

		private void ButtonRender_Click(object sender, RoutedEventArgs e)
		{
			_currentRender?.Dispose();
			_currentRender = null;

			ImagePreview.Source = null;
			ViewModel.StartPreviewRender();
		}

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (_currentRender != null)
			{
				var sfd = new SaveFileDialog
				{
					Filter = "Image Files (*.png)|*.png",
					DefaultExt = ".png"
				};

				if (sfd.ShowDialog() == true)
				{
					using var fileStream = new FileStream(sfd.FileName, FileMode.Create);
					_currentRender.Position = 0;
					_currentRender.CopyTo(fileStream);
				}
			}
			else
			{
				MessageBox.Show("Please render the image before saving", "Unable to save", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void Dispose()
		{
			_currentRender?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}