using Mosaic.GUI.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mosaic.GUI.Views
{
	public partial class PicturesView : UserControl
	{
		public PicturesView()
		{
			InitializeComponent();
		}

		private void Image_Loaded(object sender, RoutedEventArgs e)
		{
			ICommand command = ((PictureViewModel)((Image)sender).DataContext).RenderThumbnailCommand;
			if (command.CanExecute(null))
			{
				command.Execute(null);
			}
		}

		private void ListBoxImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			((PicturesViewModel)DataContext).Selected = ListBoxImages.SelectedItems.Cast<PictureViewModel>();
		}
	}
}
