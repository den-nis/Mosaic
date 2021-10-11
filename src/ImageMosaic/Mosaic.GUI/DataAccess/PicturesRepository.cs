using Mosaic.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.GUI.DataAccess
{
	public interface IPicturesRepository
	{
		event EventHandler<PictureEventArgs> OnMainChanged;
		event EventHandler<PictureEventArgs> OnPictureAdded;
		event EventHandler<PictureEventArgs> OnPictureRemoved;

		void AddPicture(PictureModel picture);
		PictureModel GetMain();
		IEnumerable<PictureModel> GetPictures();
		void RemovePicture(PictureModel picture);
		void SetMain(PictureModel target);
	}

	public class PicturesRepository : IPicturesRepository
	{
		public event EventHandler<PictureEventArgs> OnPictureAdded;
		public event EventHandler<PictureEventArgs> OnPictureRemoved;
		public event EventHandler<PictureEventArgs> OnMainChanged;

		private PictureModel _mainReference;
		private readonly HashSet<PictureModel> _pictures = new();

		public void AddPicture(PictureModel picture)
		{
			if (!_pictures.Contains(picture) && picture.LoadMeta())
			{
				_pictures.Add(picture);
				OnPictureAdded?.Invoke(this, new PictureEventArgs(picture));
			}
		}

		public void RemovePicture(PictureModel picture)
		{
			if (!_pictures.Contains(picture))
			{
				return;
			}

			_pictures.Remove(picture);
			OnPictureRemoved?.Invoke(this, new PictureEventArgs(picture));

			if (picture.IsMain)
			{
				_mainReference = null;
				OnMainChanged?.Invoke(this, new PictureEventArgs(null));
			}
		}

		public void SetMain(PictureModel target)
		{
			if (target.IsMain || !_pictures.Contains(target))
			{
				return;
			}

			if (_mainReference != null)
			{
				_mainReference.IsMain = false;
			}

			target.IsMain = true;
			_mainReference = target;
			OnMainChanged?.Invoke(this, new PictureEventArgs(target));
		}

		public IEnumerable<PictureModel> GetPictures()
		{
			return _pictures;
		}

		/// <summary>
		/// Returns null if there is no main picture
		/// </summary>
		public PictureModel GetMain() => _mainReference;
	}
}