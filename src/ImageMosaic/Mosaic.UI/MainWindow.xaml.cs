using Mosaic.Progress;
using Mosaic.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public partial class MainWindow : Window
	{
		public MainViewModel ViewModel { get; set; } 

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

		private void ViewModel_OnPreviewReady(BitmapFrame image)
		{
			Dispatcher.Invoke(() => ImagePreview.Source = image);
		}

		private void ButtonRender_Click(object sender, RoutedEventArgs e)
		{
			ImagePreview.Source = null;
			ViewModel.StartPreviewRender();
		}

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
