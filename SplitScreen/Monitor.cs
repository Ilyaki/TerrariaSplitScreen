using System.Diagnostics;
using System.Text;

namespace SplitScreen
{
	public enum LogLevel
	{
		Debug
	}

	public static class Monitor
	{
		public static void Log(string message, LogLevel logLevel = LogLevel.Debug)
		{
			Debug.WriteLine($"[SplitScreen] {message}");
		}

		public static void Log(object obj, LogLevel logLevel = LogLevel.Debug) => Log(obj.ToString(), logLevel);
	}
}
