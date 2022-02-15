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
		public void TranslateVideoToAudio(string inputPath, string outputPath)
		{
			var ffMpeg = new FFMpegConverter();
			ffMpeg.ConvertMedia(inputPath, outputPath, "wav");
		}
	}
}
