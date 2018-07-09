using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SplitScreen
{
	public enum LogLevel
	{
		Debug,
		ChatBox
	}

	public static class Monitor
	{
		private const LogLevel defaultLogLevel = LogLevel.Debug;

		public static void Log(string message, LogLevel logLevel = defaultLogLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Debug:
					Debug.WriteLine($"[SplitScreen] {message}"); return;
				case LogLevel.ChatBox:
					Terraria.Main.NewText($"[SplitScreen] {message}"); return;
			}
		}

		public static void Log(object obj, LogLevel logLevel = defaultLogLevel) => Log(obj.ToString(), logLevel);
	}
}
