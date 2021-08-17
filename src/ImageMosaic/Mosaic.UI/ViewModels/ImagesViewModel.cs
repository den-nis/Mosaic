using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
    public class ImagesViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ImageViewModel> _images = new();

        public ObservableCollection<ImageViewModel> Images => _images;

        public void OpenImages(IEnumerable<string> files)
		{
			var imageModels = files.Select(f => new ImageViewModel(f)
			{
				IsMainImage = false
			});

			foreach (var image in imageModels)
			{
				if (!_images.Contains(image))
				{
					_images.Add(image);
				}
			}

			if (!_images.Any(i => i.IsMainImage))
			{
				SetFirstAsMain();
			}
		}

		public void RemoveImage(ImageViewModel viewModel)
		{
			_images.Remove(viewModel);

			if (viewModel.IsMainImage)
			{
				SetFirstAsMain();
			}
		}

		public void SetMainImage(ImageViewModel viewModel)
		{
			foreach (var image in _images)
			{
				image.IsMainImage = false;

				if (image.Fullpath == viewModel.Fullpath)
				{
					image.IsMainImage = true;
				}
			}
		}

		private void SetFirstAsMain()
		{
			if (_images.Count > 0)
			{
				_images.First().IsMainImage = true;
			}
		}
	}
}
