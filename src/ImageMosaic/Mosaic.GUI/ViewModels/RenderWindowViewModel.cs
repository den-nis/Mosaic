using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Mosaic.GUI.DataAccess;
using Mosaic.GUI.Services;
using Mosaic.Progress;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Mosaic.GUI.ViewModels
{
	public class RenderWindowViewModel : ObservableObject
	{
		private class RenderProgress : IProgress<MosaicProgress>
		{
			private readonly DateTime _startTime = DateTime.Now;
			private readonly RenderWindowViewModel _parent;
			private readonly int _totalSteps;

			public RenderProgress(RenderWindowViewModel parent)
			{
				_parent = parent;
				_totalSteps = typeof(MosaicProgress.Steps).GetEnumValues().Length;
			}

			public void Report(MosaicProgress value)
			{
				_parent.Report(value, _totalSteps, _startTime);
			}
		}

		public ICommand RenderCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand OpenCommand { get; }

		private string _statusText;
		public string StatusText
		{
			get => _statusText;
			set
			{
				_statusText = value;
				OnPropertyChanged();
			}
		}

		private double _percentage;
		public double Percentage
		{
			get => _percentage;
			set
			{
				_percentage = value;
				PercentageText = $"{Math.Round(value * 100)}%";
				OnPropertyChanged();
			}
		}

		private string _percentageText;
		public string PercentageText
		{
			get => _percentageText;
			private set
			{
				_percentageText = value;
				OnPropertyChanged();
			}
		}

		private bool _enableOpen;
		public bool EnableOpen
		{
			get => _enableOpen;
			private set
			{
				_enableOpen = value;
				OnPropertyChanged();
			}
		}

		private bool _enableCancel;
		public bool EnableCancel
		{
			get => _enableCancel;
			private set
			{
				_enableCancel = value;
				OnPropertyChanged();
			}
		}

		private readonly CancellationTokenSource _cts = new();
		private readonly ISettingsRepository _settings;
		private readonly IMosaicRenderer _renderer;

		public RenderWindowViewModel(ISettingsRepository settings, IMosaicRenderer renderer)
		{
			_settings = settings;
			_renderer = renderer;

			_enableCancel = true;
			_enableOpen = false;

			RenderCommand = new RelayCommand(Render);
			CancelCommand = new RelayCommand(Cancel);
			OpenCommand = new RelayCommand(Open);
		}

		private async void Render()
		{
			RenderProgress progress = new(this);
			if (await _renderer.Render(progress, _cts.Token))
			{
				PercentageText = "Finished";
				EnableCancel = false;
				EnableOpen = true;
			}
		}

		private void Cancel()
		{
			_cts.Cancel();
			EnableCancel = false;
			EnableOpen = false;
			StatusText = "Render cancelled.";
		}

		private void Open()
		{
			string file = _settings.GetFileSettings().Output;

			using Process process = new();
			process.StartInfo = new ProcessStartInfo
			{
				FileName = "explorer",
				Arguments = file,
			};

			process.Start();
		}

		private void Report(MosaicProgress value, int totalSteps, DateTime startTime)
		{
			if (!_cts.IsCancellationRequested)
			{
				StringBuilder builder = new();

				TimeSpan elapsed = DateTime.Now - startTime;

				builder.AppendLine($"Saving to : {_settings.GetFileSettings().Output}");
				builder.AppendLine($"Step : {value.StepDisplayName()} ({((int)value.Step) + 1}/{totalSteps})");
				builder.AppendLine($"Elapsed time : {elapsed:hh\\:mm\\:ss}");

				StatusText = builder.ToString();
				Percentage = value.Percentage;
			}
		}
	}
}