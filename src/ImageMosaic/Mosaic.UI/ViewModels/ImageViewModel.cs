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
	public class ImageViewModel : ViewModelBase, IEquatable<ImageViewModel>
	{
		public ImageViewModel(string fullPath)
		{
			Fullpath = fullPath;
		}

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

		public bool Equals(ImageViewModel other) => Fullpath == other?.Fullpath;

		public override bool Equals(object obj) => Equals(obj as ImageViewModel);

		public override int GetHashCode() => Fullpath.GetHashCode();
	}
}
