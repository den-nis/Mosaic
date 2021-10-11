using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Mosaic.GUI.ViewModels;
using System.Windows;

namespace Mosaic.GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			DataContext = Ioc.Default.GetService<MainWindowViewModel>();
			InitializeComponent();
		}
	}
}
