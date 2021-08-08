using Microsoft.Win32;
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
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MosaicSettingsViewModel ViewModel { get; set; } = new MosaicSettingsViewModel();

		public MainWindow()
		{
			InitializeComponent();
			DataContext = ViewModel;
		}

		private void ButtonOpenImages_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Multiselect = true,
				Title = "Open images"
			};

			if (dialog.ShowDialog() == true)
			{
				ViewModel.OpenImages(dialog.FileNames);
			}
		}

		private void ButtonSetAsMain_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxImages.SelectedItem is ImageViewModel imageModel)
			{
				ViewModel.SetMainImage(imageModel);
			}
		}

		private void ButtonRemove_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxImages.SelectedItem is ImageViewModel imageModel)
			{
				ViewModel.RemoveImage(imageModel);
				ListBoxImages.SelectedIndex = 0;
			}
		}
	}
}
