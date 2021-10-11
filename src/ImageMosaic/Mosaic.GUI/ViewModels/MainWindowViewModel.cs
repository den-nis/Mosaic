using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Models;
using Mosaic.GUI.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mosaic.GUI.ViewModels
{
	public class MainWindowViewModel : ObservableObject
	{
		public PicturesViewModel PicturesViewModel { get; }
		public TileSettingsViewModel SettingsViewModel { get; }

		public ICommand OpenFileCommand { get; }
		public ICommand OpenFolderCommand { get; }
		public ICommand RenderCommand { get; }

		private readonly IDialogOpener _dialogOpener;
		private readonly IPicturesRepository _pictures;

		public MainWindowViewModel(
			PicturesViewModel picturesViewModel,
			TileSettingsViewModel tileSettingsViewModel,
			IDialogOpener opener,
			IPicturesRepository pictures,
			IMosaicRenderer renderer)
		{
			PicturesViewModel = picturesViewModel;
			SettingsViewModel = tileSettingsViewModel;

			_dialogOpener = opener;
			_pictures = pictures;

			OpenFileCommand = new RelayCommand(OpenFile);
			OpenFolderCommand = new RelayCommand(OpenFolder);
			RenderCommand = new RelayCommand(renderer.StartRender);
		}

		private void OpenFolder()
		{
			IEnumerable<string> files = _dialogOpener.OpenFolderDialogPictures();
			AddAll(files);
		}

		private void OpenFile()
		{
			IEnumerable<string> files = _dialogOpener.OpenFileDialogPictures();
			AddAll(files);
		}

		private void AddAll(IEnumerable<string> files)
		{
			if (files == null)
			{
				return;
			}

			foreach (string file in files)
			{
				_pictures.AddPicture(new PictureModel(file));
			}
		}
	}
}