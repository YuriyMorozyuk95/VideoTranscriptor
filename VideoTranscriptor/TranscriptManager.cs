using System;
using System.Threading.Tasks;

namespace VideoTranscriptor
{
	using System.IO;

	internal class TranscriptManager
	{
		private readonly VideoToAudioConverter _videoToAudioConverter;
		private readonly AudioToTextConverter _audioToTextConverter;

		public TranscriptManager()
		{
			_videoToAudioConverter = new VideoToAudioConverter();
			_audioToTextConverter = new AudioToTextConverter();
			_audioToTextConverter.TextRecognized += AudioToTextConverter_TextRecognized;
		}

		public event EventHandler FileTranslatedEvent;
		public event EventHandler LogEvent;

		public async Task TranscriptVideoFiles(string[] videoLocations, string outputLocation)
		{
			for (var index = 0; index < videoLocations.Length; index++)
			{
				var videoPath = videoLocations[index];
				await TranscriptVideo(videoPath, outputLocation);
				FileTranslatedEvent?.Invoke(index + 1, EventArgs.Empty);
			}
		}

		private async Task TranscriptVideo(string videoPath, string outputLocation)
		{
			var fileName = Path.GetFileNameWithoutExtension(videoPath);

			var audioFileName = $"{fileName}_audio.wav";
			var audioFilePath = Path.Combine(outputLocation, audioFileName);
			await _videoToAudioConverter.ConvertVideoToAudio(videoPath, audioFilePath);
			LogEvent?.Invoke($"audio file {audioFileName} created", EventArgs.Empty);

			var transcript = await _audioToTextConverter.Convert(audioFilePath);
			
			var textFileName = $"{fileName}_transcript.txt";
			var textFilePath = Path.Combine(outputLocation, textFileName);

			File.AppendAllText(textFilePath, transcript);
			LogEvent?.Invoke($"text transcript file {textFileName} created", EventArgs.Empty);

			File.Delete(audioFilePath);
			LogEvent?.Invoke($"audio file {audioFileName} removed", EventArgs.Empty);
		}

		private void AudioToTextConverter_TextRecognized(object sender, EventArgs e)
		{
			var text = sender as string;
			LogEvent?.Invoke($"transcript: {sender}", e);
		}
	}
}


