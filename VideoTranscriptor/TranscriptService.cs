using System;
using System.Threading.Tasks;

namespace VideoTranscriptor
{
	internal class TranscriptService
	{
		private readonly VideoToAudioConverter _videoToAudioConverter;
		private readonly AudioToTextConverter _audioToTextConverter;

		public TranscriptService()
		{
			_videoToAudioConverter = new VideoToAudioConverter();
			_audioToTextConverter = new AudioToTextConverter();
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
			
		}
	}
}


