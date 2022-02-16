using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoTranscriptor.Annotations;
using Microsoft.Win32;

namespace VideoTranscriptor
{
	using System.Linq;
	using System.Windows.Controls;
	using WinForms = System.Windows.Forms;
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	[UsedImplicitly]
	public partial class MainWindow : INotifyPropertyChanged
	{
		private string _outputFolder;
		private int _itemProcessed;

		public MainWindow()
		{
			InitializeComponent();

			ItemProcessed = 0;
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

			foreach (var file in openFileDialog.FileNames)
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

		private async void TranscriptBtn_OnClick(object sender, RoutedEventArgs e)
		{
			if (VideoPathList.Count == 0 || string.IsNullOrWhiteSpace(OutputFolder))
			{
				MessageBox.Show("please setup all fields");
			}

			var transcriptService = new TranscriptManager();
			transcriptService.FileTranslatedEvent += TranscriptServiceFileTranslatedEvent;
			transcriptService.LogEvent += TranscriptServiceLogEvent;

			ProgressBar.Maximum = VideoPathList.Count;
			ItemProcessed = 0;

			try
			{
				await transcriptService.TranscriptVideoFiles(
					VideoPathList.ToArray(),
					OutputFolder);

			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
			}
			finally
			{
				MessageBox.Show("Done!");
			}
		}

		private void TranscriptServiceLogEvent(object sender, EventArgs e)
		{
			OutputLog += (string)sender + Environment.NewLine;
		}

		private void TranscriptServiceFileTranslatedEvent(object sender, EventArgs e)
		{
			ItemProcessed = (int)sender;
		}

		private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			textBox.ScrollToEnd();
		}
	}

	public partial class MainWindow : INotifyPropertyChanged
	{
		private string _outputLog;
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

		public int ItemProcessed
		{
			get => _itemProcessed;
			set
			{
				_itemProcessed = value;
				OnPropertyChanged();
			}
		}

		public string OutputLog
		{
			get => _outputLog;
			set
			{
				_outputLog = value;
				OnPropertyChanged();
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
