using Microsoft.Toolkit.Mvvm.ComponentModel;
using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Models;
using System;
using System.Text;

namespace Mosaic.GUI.ViewModels
{
	public class TileSettingsViewModel : ObservableObject, IDisposable
	{
		private const int MAX_SIZE_TILES = 120;
		private const int MIN_SIZE_TILES = 10;
		private const float DEFAULT_SLIDER = 0.5f;

		private readonly ISettingsRepository _repository;
		private readonly IPicturesRepository _pictures;

		private bool _enabled;
		public bool Enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
				OnPropertyChanged();
			}
		}

		private float _tickSize;
		public float TickSize
		{
			get => _tickSize;
			set
			{
				_tickSize = value;
				OnPropertyChanged();
			}
		}

		private float _minSize;
		public float MinSize
		{
			get => _minSize;
			set
			{
				_minSize = value;
				OnPropertyChanged();
			}
		}

		private float _maxSize;
		public float MaxSize
		{
			get => _maxSize;
			set
			{
				_maxSize = value;
				OnPropertyChanged();
			}
		}

		private float _size;
		public float Size
		{
			get => _size;
			set
			{
				if (_size != value)
				{
					_size = value;
					_repository.GetTileSettings().Size = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Info));
				}
			}
		}

		private bool _includeMain;
		public bool IncludeMain
		{
			get => _includeMain;
			set
			{
				_includeMain = value;
				_repository.GetTileSettings().IncludeMain = value;
				OnPropertyChanged();
			}
		}

		private bool _enableRotation;
		public bool EnableRotation
		{
			get => _enableRotation;
			set
			{
				_enableRotation = value;
				_repository.GetTileSettings().EnableRotation = value;
				OnPropertyChanged();
			}
		}

		private bool _enableMirror;
		public bool EnableMirror
		{
			get => _enableMirror;
			set
			{
				_enableMirror = value;
				_repository.GetTileSettings().EnableMirror = value;
				OnPropertyChanged();
			}
		}

		public string Info => GetInfoString();

		public TileSettingsViewModel(ISettingsRepository repository, IPicturesRepository pictures)
		{
			_repository = repository;
			_pictures = pictures;
			_repository.SettingsUpdated += SettingsUpdated;
			_pictures.OnMainChanged += OnMainChanged;

			RefreshMainPictureData();
		}

		private void OnMainChanged(object sender, PictureEventArgs e)
		{
			RefreshMainPictureData();
		}

		private void SettingsUpdated(object sender, EventArgs e)
		{
			OnPropertyChanged(nameof(Info));
		}

		private void RefreshMainPictureData()
		{
			PictureModel main = _pictures.GetMain();

			if (main != null)
			{
				float current = DEFAULT_SLIDER;

				if (MinSize != 0 && MaxSize != 0)
				{
					current = (Size - MinSize) / (MaxSize - MinSize);
				}

				int longSide = Math.Max(main.Width, main.Height);
				MaxSize = (float)MAX_SIZE_TILES / longSide;
				MinSize = (float)MIN_SIZE_TILES / longSide;
				TickSize = (MaxSize - MinSize) / 100f;
				Size = MinSize + current * (MaxSize - MinSize);

				Enabled = true;
				OnPropertyChanged(nameof(Info));
			}
			else
			{
				Enabled = false;
			}
		}

		private string GetInfoString()
		{
			PictureModel main = _pictures.GetMain();
			if (main == null)
			{
				return string.Empty;
			}

			StringBuilder sb = new();
			AdvancedSettings advanced = _repository.GetAdvancedSettings();
			int columns = (int)(main.Width * Size);
			int rows = (int)(main.Height * Size);

			sb.AppendLine($"Columns: {columns}");
			sb.AppendLine($"Rows: {rows}");
			sb.AppendLine($"Render size: {columns * advanced.Resolution}x{rows * advanced.Resolution}");

			return sb.ToString();
		}

		public void Dispose()
		{
			_repository.SettingsUpdated -= SettingsUpdated;
			_pictures.OnMainChanged -= OnMainChanged;

			GC.SuppressFinalize(this);
		}
	}
}
