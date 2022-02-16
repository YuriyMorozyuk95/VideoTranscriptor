using System.Threading.Tasks;

namespace VideoTranscriptor
{
	using System;

	internal class AudioToTextConverter
	{
		private readonly RecognitionService _recognizerService;

		public AudioToTextConverter()
		{
			_recognizerService = new RecognitionService();
			_recognizerService.TextRecognized += RecognizerServiceTextRecognized;
		}

		public event EventHandler TextRecognized;

		public Task<string> Convert(string pathToAudio)
		{
			return _recognizerService.RecognizeAudio(pathToAudio);
		}

		private void RecognizerServiceTextRecognized(object sender, System.EventArgs e)
		{
			TextRecognized?.Invoke(sender, e);
		}
	}
}
