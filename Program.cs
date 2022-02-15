using System;
using System.IO;
using NReco.VideoConverter;

namespace VideoToTranscript
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var input = "F:\\TestVideo\\Test.mp4";
			var output = "F:\\TestVideo\\Test.wav";
			var ffMpeg = new FFMpegConverter();
			ffMpeg.ConvertMedia(input, output, "wav");
		}

		private void TestConvert3gpToMp4()
		{
			
		}
	}
}
