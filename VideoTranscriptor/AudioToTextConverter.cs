using System.Threading.Tasks;

namespace VideoTranscriptor
{
	internal class AudioToTextConverter
	{
		private readonly RecognitionService _recognizerService;

		public AudioToTextConverter()
		{
			_recognizerService = new RecognitionService();
		}

		public Task<string> Convert(string pathToAudio)
		{
			return _recognizerService.RecognizeAudio(pathToAudio);
		}
	}
}
