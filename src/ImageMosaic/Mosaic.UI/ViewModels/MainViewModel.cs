using ImageMagick;
using Mosaic.Progress;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Mosaic.UI.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public bool Busy { get; set; }

		public event Action<MosaicProgress> OnProgressChanged;
		public event Action<BitmapFrame> OnPreviewReady;

		private readonly ImagesViewModel _imagesVm;
		private readonly EffectsViewModel _effectsVm;
		private readonly CellsViewModel _cellsVm;

		public MainViewModel(
			ImagesViewModel images,
			EffectsViewModel effects,
			CellsViewModel cells
			)
		{
			_imagesVm = images;
			_effectsVm = effects;
			_cellsVm = cells;
		}

		public async void StartPreviewRender()
		{
			if (!CanRender()) return;
			Busy = true;

			var progress = new Progress<MosaicProgress>();
			progress.ProgressChanged += (_, p) => OnProgressChanged?.Invoke(p);

			var image = new MosaicImage
			{
				Files = _imagesVm.Images.Where(i => !i.IsMainImage).Select(f => f.Fullpath).ToList(),
				Main = _imagesVm.Images.First(i => i.IsMainImage).Fullpath,
				Cols = _cellsVm.Columns,
				Rows = _cellsVm.Rows,
				Resolution = _cellsVm.Resolution,
				Progress = progress,

				Red = _effectsVm.Red * 256,
				Green = _effectsVm.Green * 256,
				Blue = _effectsVm.Blue * 256,

				Contrast = _effectsVm.Contrast,
				Brightness = _effectsVm.Brightness,
			};

			var result = await image.Render();

			await SetPreviewAsync(result.ImageStream);

			Busy = false;
		}

		private bool CanRender()
		{
			if (Busy)
				return false;

			if (_imagesVm.Images.Count < 2)
				return false;

			return true;
		}

		private Task SetPreviewAsync(Stream image)
		{
			return Task.Run(() =>
			{
				var bitmap = BitmapFrame.Create(image, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
				OnPreviewReady?.Invoke(bitmap);
			});
		}
	}
}
