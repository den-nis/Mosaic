using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mosaic.GUI.Services
{
	public interface IWindowOpener
	{
		void OpenAdvancedWindow();
		void OpenRenderWindow();
	}

	public class WindowOpener : IWindowOpener
	{
		public void OpenRenderWindow()
		{
			RenderWindow renderWindow = new();
			renderWindow.Owner = Application.Current.MainWindow;
			renderWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			renderWindow.ShowDialog();
		}

		public void OpenAdvancedWindow()
		{
			throw new NotImplementedException();
		}
	}
}