using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace VideoTranscriptor
{
	using System;

	public class AzureCognitiveServicesConstants
	{
		public const string SubscriptionKey = "c7e4cd32c100421da93913fdbd0f7284";
		public const string Region = "norwayeast";
	}

	internal class RecognitionService
	{
		private TaskCompletionSource<string> _recognitionTaskCompletionSource;
		private StringBuilder _textRecognitionBuilder;

		public event EventHandler TextRecognized;

		public async Task<string> RecognizeAudio(string audioInputFile)
		{
			_textRecognitionBuilder = new StringBuilder();
			string recognizeText;
			_recognitionTaskCompletionSource = new TaskCompletionSource<string>();

			var config = SpeechConfig.FromSubscription(
				AzureCognitiveServicesConstants.SubscriptionKey,
				AzureCognitiveServicesConstants.Region);

			using (var audioConfig = AudioConfig.FromWavFileInput(audioInputFile))
			using (var recognizer = new SpeechRecognizer(config, audioConfig))
			{
				await recognizer.StartContinuousRecognitionAsync();
				recognizer.Recognized += Recognizer_Recognized;
				recognizer.SpeechEndDetected += Recognizer_SpeechEndDetected;
				recognizer.Canceled += Recognizer_Canceled;

				recognizeText = await _recognitionTaskCompletionSource.Task;

				await recognizer.StopContinuousRecognitionAsync();
			}

			return recognizeText;
		}


		private void Recognizer_Canceled(object sender, SpeechRecognitionCanceledEventArgs e)
		{
			var finalText = _textRecognitionBuilder.ToString();
			_recognitionTaskCompletionSource.TrySetResult(finalText);
		}

		private void Recognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
		{
			var finalText = _textRecognitionBuilder.ToString();
			_recognitionTaskCompletionSource.TrySetResult(finalText);
		}

		private void Recognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
		{
			_textRecognitionBuilder.AppendLine(e.Result.Text);
			TextRecognized?.Invoke(e.Result.Text, EventArgs.Empty);
		}
	}
}
