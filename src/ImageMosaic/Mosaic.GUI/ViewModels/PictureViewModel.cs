using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mosaic.GUI.ViewModels
{
	public class PictureViewModel : ObservableObject
	{
		private const int THUMBNAIL_SIZE = 64;

		public ICommand RenderThumbnailCommand { get; }
		public PictureModel Model { get; set; }
		public string Filename => Path.GetFileName(Model.FullPath);
		public string Tag => IsMain ? "Main" : string.Empty;

		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged();
			}
		}

		private bool _isMain;
		public bool IsMain
		{
			get => _isMain;
			set
			{
				_isMain = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Tag));
			}
		}

		private ImageSource _thumbnail;
		public ImageSource Thumbnail
		{
			get => _thumbnail;
			set
			{
				_thumbnail = value;
				OnPropertyChanged();
			}
		}

		private readonly IPicturesRepository _repository;

		public PictureViewModel(PictureModel model, IPicturesRepository repository)
		{
			Model = model;
			_repository = repository;

			RenderThumbnailCommand = new RelayCommand(RenderThumbnailAsync);
			_repository.OnMainChanged += OnMainChanged;
		}

		private void OnMainChanged(object sender, PictureEventArgs e)
		{
			IsMain = e.Picture == Model;
		}

		public void Remove()
		{
			_repository.RemovePicture(Model);
		}

		public void SetMain()
		{
			_repository.SetMain(Model);
			_isMain = true;
		}

		public void RenderThumbnailAsync() => Task.Run(RenderThumbnail);

		public void RenderThumbnail()
		{
			BitmapImage thumbnail = new();
			thumbnail.BeginInit();
			thumbnail.UriSource = new Uri(Model.FullPath);
			thumbnail.DecodePixelWidth = THUMBNAIL_SIZE;
			thumbnail.CacheOption = BitmapCacheOption.OnLoad;
			thumbnail.EndInit();
			thumbnail.Freeze();

			Thumbnail = thumbnail;
		}
	}
}