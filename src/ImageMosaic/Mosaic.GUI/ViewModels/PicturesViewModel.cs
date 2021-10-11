using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Mosaic.GUI.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Mosaic.GUI.ViewModels
{
	public class PicturesViewModel : ObservableObject, IDisposable
	{
		public ICommand RemoveCommand { get; }
		public ICommand SetMainCommand { get; }

		public IEnumerable<PictureViewModel> Selected { get; set; }
		public ObservableCollection<PictureViewModel> Pictures { get; } = new();

		private readonly IPicturesRepository _picturesRepository;

		public PicturesViewModel(IPicturesRepository picturesRepository)
		{
			_picturesRepository = picturesRepository;

			_picturesRepository.OnPictureAdded += OnPictureAdded;
			_picturesRepository.OnPictureRemoved += OnPictureRemoved;

			RemoveCommand = new RelayCommand(Remove);
			SetMainCommand = new RelayCommand(SetMain);
		}

		//TODO: Make this method/command faster
		private void Remove()
		{
			List<PictureViewModel> selectedPictures = new(Selected ?? Enumerable.Empty<PictureViewModel>());
			foreach (PictureViewModel picture in selectedPictures)
			{
				picture.Remove();
			}
		}

		private void SetMain()
		{
			Selected?.FirstOrDefault()?.SetMain();
		}

		private void OnPictureAdded(object sender, PictureEventArgs args)
		{
			Pictures.Add(new PictureViewModel(args.Picture, _picturesRepository));
		}

		private void OnPictureRemoved(object sender, PictureEventArgs args)
		{
			PictureViewModel picture = Pictures.First(p => p.Model == args.Picture);
			Pictures.Remove(picture);
		}

		public void Dispose()
		{
			_picturesRepository.OnPictureAdded -= OnPictureAdded;
			_picturesRepository.OnPictureRemoved -= OnPictureRemoved;

			GC.SuppressFinalize(this);
		}
	}
}