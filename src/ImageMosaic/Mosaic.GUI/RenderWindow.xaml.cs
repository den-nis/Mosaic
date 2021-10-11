using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Mosaic.GUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Mosaic.GUI
{
	/// <summary>
	/// Interaction logic for RenderWindow.xaml
	/// </summary>
	public partial class RenderWindow : Window
	{
		public RenderWindow()
		{
			DataContext = Ioc.Default.GetService<RenderWindowViewModel>();
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ICommand command = ((RenderWindowViewModel)DataContext).RenderCommand;
			if (command.CanExecute(null))
			{
				command.Execute(null);
			}
		}
	}
}
