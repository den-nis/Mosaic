using Mosaic.GUI.Models;
using System;

namespace Mosaic.GUI.DataAccess
{
	public class PictureEventArgs : EventArgs
	{
		public PictureModel Picture { get; }

		public PictureEventArgs(PictureModel picture)
		{
			Picture = picture;
		}
	}
}