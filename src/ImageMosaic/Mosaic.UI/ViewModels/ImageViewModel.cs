using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.UI.ViewModels
{
	public class ImageViewModel
	{
		public string Filename { get; set; }
		public bool IsMainImage { get; set; }
		public string Tag => IsMainImage ? "Main" : string.Empty;
	}
}
