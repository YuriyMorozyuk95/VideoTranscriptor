using System;
using System.Threading.Tasks;

namespace VideoTranscriptor
{
	internal class TranscriptService
	{
		public TranscriptService()
		{
				
		}

		public async Task TranscriptVideoFiles(string[] videoLocations, string output, Action callBack)
		{
			foreach (var videoPath in videoLocations)
			{
				await TranscriptVideo(videoPath);
				callBack();
			}
		}

		private async Task TranscriptVideo(string path)
		{
			await Task.Delay(1000);
		}
	}
}

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapplo.Confluence;
using Dapplo.Confluence.Query;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

using NReco.VideoConverter;

namespace VideoToTranscriptFotNet
{
	internal class Program
	{
		private static string _finalText;

		static async Task Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var input = "F:\\TestVideo\\Test.mp4";
			var output = "F:\\TestVideo\\Test.wav";
			var outputText = "F:\\TestVideo\\Test.txt";

			var ffMpeg = new FFMpegConverter();
			ffMpeg.ConvertMedia(input, output, "wav");

			await Test(output);

			File.AppendAllText(outputText, _finalText);
			//await RecognizeSpeechAsync(output);
			Console.WriteLine("Please press <Return> to continue.");
			Console.ReadLine();
		}

		async static Task FromStream(string input)
		{
			var config = SpeechConfig.FromSubscription("c7e4cd32c100421da93913fdbd0f7284", "norwayeast");

			var reader = new BinaryReader(File.OpenRead(input));
			using (var audioInputStream = AudioInputStream.CreatePushStream())
			using (var audioConfig = AudioConfig.FromStreamInput(audioInputStream))
			using (var recognizer = new SpeechRecognizer(config, audioConfig))
			{
				byte[] readBytes;
				do
				{
					readBytes = reader.ReadBytes(1024);
					audioInputStream.Write(readBytes, readBytes.Length);
				} while (readBytes.Length > 0);

				var result = await recognizer.RecognizeOnceAsync();
				Console.WriteLine($"RECOGNIZED: Text={result.Text}");
			}
		}

		private static async Task RecognizeSpeechAsync(string input)
		{
			var config = SpeechConfig.FromSubscription("c7e4cd32c100421da93913fdbd0f7284", "norwayeast");

			using (var audioConfig = AudioConfig.FromWavFileInput(input))
			using (var recognizer = new SpeechRecognizer(config, audioConfig))
			{
				var result = await recognizer.RecognizeOnceAsync();

				if (result.Reason == ResultReason.RecognizedSpeech)
				{
					Console.WriteLine($"We recognized: {result.Text}");
				}
				else if (result.Reason == ResultReason.NoMatch)
				{
					Console.WriteLine($"NOMATCH: Speech could not be recognized.");
				}
				else if (result.Reason == ResultReason.Canceled)
				{
					var cancellation = CancellationDetails.FromResult(result);
					Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

					if (cancellation.Reason == CancellationReason.Error)
					{
						Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
						Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
						Console.WriteLine($"CANCELED: Did you update the subscription info?");
					}
				}
			}
		}

		private static async Task Test(string input)
		{
			var config = SpeechConfig.FromSubscription("c7e4cd32c100421da93913fdbd0f7284", "norwayeast");

			using (var audioConfig = AudioConfig.FromWavFileInput(input))
			using (var recognizer = new SpeechRecognizer(config, audioConfig))
			{
				await recognizer.StartContinuousRecognitionAsync();
				recognizer.Recognized += Recognizer_Recognized;
				recognizer.SpeechEndDetected += Recognizer_SpeechEndDetected;
				recognizer.Canceled += Recognizer_Canceled;

				//TODO add task compilation source
 				await Task.Delay(TimeSpan.FromMinutes(2));

                await recognizer.StopContinuousRecognitionAsync();
			}
		}

		private static void Recognizer_Canceled(object sender, SpeechRecognitionCanceledEventArgs e)
		{
			_finalText = builder.ToString();
		}

		private static void Recognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
		{
			_finalText = builder.ToString();
		}

		public static StringBuilder builder { get; set; } = new StringBuilder();

		private static void Recognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
		{
			builder.AppendLine(e.Result.Text);
		}
	}
}

