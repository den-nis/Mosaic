using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Models;
using Mosaic.Progress;
using Mosaic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaic.GUI.Services
{
	public interface IMosaicRenderer
	{
		void StartRender();
		Task<bool> Render(IProgress<MosaicProgress> progress, CancellationToken token);
	}

	public class MosaicRenderer : IMosaicRenderer
	{
		private const int MIN_AMOUNT_PICTURES = 2;

		private readonly IPicturesRepository _pictures;
		private readonly ISettingsRepository _settings;
		private readonly IDialogOpener _dialogOpener;
		private readonly IWindowOpener _windowOpener;

		public MosaicRenderer(IPicturesRepository pictures, ISettingsRepository settings, IDialogOpener dialogOpener, IWindowOpener windowOpener)
		{
			_pictures = pictures;
			_settings = settings;
			_dialogOpener = dialogOpener;
			_windowOpener = windowOpener;
		}

		public void StartRender()
		{
			var unavailable = _pictures.GetUnavailablePictures();
			if (unavailable.Any())
			{
				UnavailableDialog(unavailable);
				return;
			}

			if (_pictures.GetMain() == null)
			{
				_dialogOpener.ErrorDialog("There is no main image selected");
				return;
			}

			if (_pictures.GetAvailablePictures().Count() < MIN_AMOUNT_PICTURES)
			{
				_dialogOpener.ErrorDialog($"There need to be atleast {MIN_AMOUNT_PICTURES} pictures");
				return;
			}

			string saveAs = _dialogOpener.SaveFileDialogMainPicture();

			if (saveAs != null)
			{
				_settings.GetFileSettings().Output = saveAs;
				_windowOpener.OpenRenderWindow();
			}
		}

		public async Task<bool> Render(IProgress<MosaicProgress> progress, CancellationToken token)
		{
			string output = _settings.GetFileSettings().Output;
			bool includeMain = _settings.GetTileSettings().IncludeMain;

			MosaicImage image = new(BuildSettings())
			{
				MainPicture = new PictureSourceFile(_pictures.GetMain().FullPath),
				TilePictures = _pictures.GetAvailablePictures()
				.Where(p => !p.IsMain || includeMain)
				.Select(p => new PictureSourceFile(p.FullPath))
			};

			try
			{
				using RenderResult result = await Task.Run(() => image.Render(progress, token));
				await Task.Run(() => result.Write(output), token);
			}
			catch (OperationCanceledException)
			{
				return false;
			}
			catch (AggregateException aggregateException)
			{
				if (aggregateException.InnerExceptions.All(i => i is OperationCanceledException))
				{
					return false;
				}

				throw;
			}

			return true;
		}

		private RenderSettings BuildSettings()
		{
			TileSettings tileSettings = _settings.GetTileSettings();
			AdvancedSettings advancedSettings = _settings.GetAdvancedSettings();

			return new RenderSettings
			{
				Size = tileSettings.Size,
				UseMirror = tileSettings.EnableMirror,
				UseRotation = tileSettings.EnableRotation,

				RepeatRadius = -1,
				Resolution = advancedSettings.Resolution,
				SamplesPerTile = 4,
				UseAverageSamples = true,
				UseGridSearch = false,
				CropMode = CropMode.Center,
			};
		}

		private void UnavailableDialog(IEnumerable<PictureModel> unavailable)
        {
			StringBuilder sb = new("The following images are unavailable");
			sb.AppendLine();

			foreach (var image in unavailable)
			{
				sb.AppendLine($" - {image.FullPath}");
			}

			_dialogOpener.ErrorDialog(sb.ToString());
		}
	}
}