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
