using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.GUI.Models
{
	public class TileSettings : ObservableObject
	{
		private float _size;
		public float Size
		{
			get => _size;
			set
			{
				_size = value;
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
				OnPropertyChanged();
			}
		}

		private bool _includeMain;
		public bool IncludeMain
		{
			get => _includeMain;
			set
			{
				_includeMain = value;
				OnPropertyChanged();
			}
		}
	}
}