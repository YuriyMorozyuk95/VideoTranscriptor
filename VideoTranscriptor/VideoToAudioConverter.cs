using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranscriptor
{
	using NReco.VideoConverter;

	internal class VideoToAudioConverter
	{
		public Task ConvertVideoToAudio(string inputPath, string outputPath)
		{
			var ffMpeg = new FFMpegConverter();
			ffMpeg.ConvertMedia(inputPath, outputPath, "wav");

			return Task.CompletedTask;
		}
	}
}
