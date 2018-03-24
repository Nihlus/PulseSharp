using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MP3Sharp;
using PulseSharp.Context;
using PulseSharp.Enums;
using PulseSharp.MainLoopAbstractions;
using PulseSharp.Simple;
using PulseSharp.Structures;

namespace PulseSharp.Playground
{
	class Program
	{
		static async Task Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			PlaySimpleAudio();
			//await PlayComplexAudio();
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine(e.ExceptionObject);
		}

		static async Task PlayComplexAudio()
		{
			using (var mainloop = new ThreadedMainLoop())
			using (var context = new PulseContext(mainloop, "PulseSharp.Playground"))
			{
				await context.ConnectAsync();

				var info = await context.GetServerInfoAsync();

				await context.DisconnectAsync();
			}
		}

		static void PlaySimpleAudio()
		{
			var localDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var mp3 = new MP3Stream(Path.Combine(localDir, "sample.mp3"));

			var sampleSpec = new SampleSpecification
			{
				ChannelCount = (byte)mp3.ChannelCount,
				Format = SampleFormat.S16LE,
				Rate = (uint)mp3.Frequency
			};

			using (var connection = new SimplePulseStream
			(
				"PulseSharp.Playground",
				"Playback",
				StreamDirection.Playback,
				sampleSpec
			))
			{
				var buffer = new byte[1024];
				while (mp3.Position < mp3.Length)
				{
					// Write data in 1024-byte chunks
					mp3.Read(buffer, 0, buffer.Length);

					connection.Write(buffer);
				}
			}
		}
	}
}