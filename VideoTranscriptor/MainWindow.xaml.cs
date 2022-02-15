using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace VideoTranscriptor
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using Annotations;
	using Microsoft.Win32;
	using WinForms = System.Windows.Forms;
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private string _outputFolder;

		public MainWindow()
		{
			InitializeComponent();
		}

		public ObservableCollection<string> VideoPathList { get; set; } = new ObservableCollection<string>();

		public string OutputFolder
		{
			get => _outputFolder;
			set
			{
				_outputFolder = value;
				OnPropertyChanged();
			}
		}


		private void SelectVideoBtn_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Multiselect = true,
				Filter = "Video files (*.mp4)|*.mp4",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};

			if (openFileDialog.ShowDialog() != true) return;
			
			VideoPathList.Clear();

			foreach(var file in openFileDialog.FileNames)
			{
				VideoPathList.Add(file);
			}
		}

		private void SelectOutputPathBtn_OnClick(object sender, RoutedEventArgs e)
		{
			using (var fbd = new WinForms.FolderBrowserDialog())
			{
				WinForms.DialogResult result = fbd.ShowDialog();

				if (result == WinForms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					OutputFolder = fbd.SelectedPath;
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
