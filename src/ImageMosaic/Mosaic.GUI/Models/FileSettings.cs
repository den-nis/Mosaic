using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.GUI.Models
{
	public class FileSettings : ObservableObject
	{
		private string _ouput;
		public string Output
		{
			get => _ouput;
			set
			{
				_ouput = value;
				OnPropertyChanged();
			}
		}
	}
}