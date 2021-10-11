using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Mosaic.GUI.Services
{
	public interface IDialogOpener
	{
		void ErrorDialog(string message);
		IEnumerable<string> OpenFileDialogPictures();
		IEnumerable<string> OpenFolderDialogPictures();
		string SaveFileDialogMainPicture();
	}

	public class DialogOpener : IDialogOpener
	{
		private readonly string Filter = @"Image Files(*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp|All files (*.*)|*.*";

		public IEnumerable<string> OpenFileDialogPictures()
		{
			OpenFileDialog openFileDialog = new()
			{
				Multiselect = true,
				Filter = Filter,
			};

			return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileNames : null;
		}

		public IEnumerable<string> OpenFolderDialogPictures()
		{
			FolderBrowserDialog folderDialog = new();
			if (folderDialog.ShowDialog() == DialogResult.OK)
			{
				string[] files = Directory.GetFiles(folderDialog.SelectedPath, "*.*", SearchOption.AllDirectories);
				return files;
			}

			return null;
		}

		public string SaveFileDialogMainPicture()
		{
			SaveFileDialog saveFileDialog = new()
			{
				AddExtension = true,
				DefaultExt = "png",
				Filter = Filter,
				CheckPathExists = true,
				FileName = "Render.png",
			};

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				return saveFileDialog.FileName;
			}

			return null;
		}

		public void ErrorDialog(string message)
		{
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}