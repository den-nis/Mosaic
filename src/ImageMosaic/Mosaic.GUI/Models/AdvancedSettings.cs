using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.GUI.Models
{
	//TODO: Implement advanced settings menu
	public class AdvancedSettings : ObservableObject
	{
		public AdvancedSettings()
		{
			Resolution = 64;
		}

		private int _resolution;
		public int Resolution
		{
			get => _resolution;
			set
			{
				_resolution = value;
				OnPropertyChanged();
			}
		}
	}
}