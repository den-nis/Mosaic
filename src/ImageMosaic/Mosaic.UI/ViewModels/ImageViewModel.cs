using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class ImageViewModel : INotifyPropertyChanged, IEquatable<ImageViewModel>
	{
		private string _fullpath;
		public string Fullpath
		{
			get => _fullpath;
			set
			{
				_fullpath = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Filename));
			}
		}

		private bool _isMainImage;
		public bool IsMainImage
		{
			get => _isMainImage;
			set
			{
				_isMainImage = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Tag));
			}
		}

		public string Filename => Path.GetFileName(Fullpath);
		public string Tag => IsMainImage ? "Main" : string.Empty;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool Equals(ImageViewModel other) => Fullpath == other.Fullpath;
	}
}
