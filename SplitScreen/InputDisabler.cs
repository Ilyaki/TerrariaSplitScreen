using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AutoHotkey.Interop;

namespace SplitScreen.Mice
{
	class InputDisabler
	{
		//Raw input can still receive input even when this is running

		//https://github.com/amazing-andrew/AutoHotkey.Interop
		//https://github.com/Ilyaki/SplitScreen/blob/master/SplitScreen/Mice/MouseDisabler.cs

		private static AutoHotkeyEngine ahk;
		public static bool IsAutoHotKeyNull => ahk == null;

		#region Windows API
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(int hwnd);

		[DllImport("user32.dll")]
		private static extern int GetDesktopWindow();

		[DllImport("user32.dll")]
		static extern int ShowCursor(bool bShow);
		#endregion

		public InputDisabler()
		{
			try
			{
				ahk = new AutoHotkeyEngine();
				ahk.ExecRaw("*RButton:: return");//The star means it will disable even with modifier keys e.g. Shift
				ahk.ExecRaw("*LButton:: return");
				ahk.ExecRaw("*MButton:: return");
				ahk.ExecRaw("*XButton1:: return");
				ahk.ExecRaw("*XButton2:: return");
				//ahk.ExecRaw("*WheelDown:: return"); //Player can scroll very fast such that AutoHotKey is overloaded, so dont lock scroll wheel
				//ahk.ExecRaw("*WheelUp:: return");
				//ahk.ExecRaw("*WheelLeft:: return");
				//ahk.ExecRaw("*WheelRight:: return");

				//Prevents accidentally opening start menu
				ahk.ExecRaw("*Lwin:: return");
				ahk.ExecRaw("*Rwin:: return");

				//(Doesn't seem to prevent accidentally alt+tab)
				ahk.ExecRaw("*Alt:: return");
				ahk.ExecRaw("*Control:: return");
				ahk.ExecRaw("*Shift:: return");
				ahk.Suspend();
			}
			catch
			{
				Monitor.Log("Could not load Mouse Disabler. This is probably not the first instance");//TODO: info level
			}
		}

		public void Lock()
		{
			ahk?.UnSuspend();
			SetForegroundWindow(GetDesktopWindow());//Loses focus of all windows, without minimizing
			if (ahk != null) System.Windows.Forms.Cursor.Hide();//Only works if the game window in the top left corner (0,0)
		}

		public void Unlock()
		{
			if (ahk != null)
			{
				ahk.Suspend();
				System.Windows.Forms.Cursor.Show();

				/* https://msdn.microsoft.com/en-us/library/windows/desktop/ms633539.aspx
					An application cannot force a window to the foreground while the user is working with another window. Instead, Windows flashes the taskbar button of the window to notify the user.
						^^^ (doesn't work) */
				try
				{
					SetForegroundWindow((int)Terraria.Main.instance.Window.Handle);
				}
				catch (Exception) { }


			}
		}
	}
}
