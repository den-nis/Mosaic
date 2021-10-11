using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Mosaic.GUI.ViewModels;
using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Services;
using System;
using System.Windows;

namespace Mosaic.GUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public App()
        {
            var services = ConfigureServices();
            Ioc.Default.ConfigureServices(services);
            InitializeComponent();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IPicturesRepository, PicturesRepository>();
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            services.AddSingleton<IMosaicRenderer, MosaicRenderer>();
            services.AddSingleton<IDialogOpener, DialogOpener>();
            services.AddSingleton<IWindowOpener, WindowOpener>();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<RenderWindowViewModel>();
            services.AddTransient<PicturesViewModel>();
            services.AddTransient<TileSettingsViewModel>();

            return services.BuildServiceProvider();
        }
	}
}
