using Mosaic.GUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.GUI.DataAccess
{
	public interface ISettingsRepository
	{
		event EventHandler<EventArgs> SettingsUpdated;

		TileSettings GetTileSettings();
		AdvancedSettings GetAdvancedSettings();
		FileSettings GetFileSettings();
	}

	public class SettingsRepository : ISettingsRepository
	{
		public event EventHandler<EventArgs> SettingsUpdated;

		private readonly TileSettings _tileSettings = new();
		private readonly AdvancedSettings _advancedSettings = new();
		private readonly FileSettings _fileSettings = new();

		public SettingsRepository()
		{
			_tileSettings.PropertyChanged += (s, a) => SettingsUpdated?.Invoke(s, a);
			_advancedSettings.PropertyChanged += (s, a) => SettingsUpdated?.Invoke(s, a);
			_fileSettings.PropertyChanged += (s, a) => SettingsUpdated?.Invoke(s, a);
		}

		public TileSettings GetTileSettings() => _tileSettings;

		public AdvancedSettings GetAdvancedSettings() => _advancedSettings;

		public FileSettings GetFileSettings() => _fileSettings;
	}
}