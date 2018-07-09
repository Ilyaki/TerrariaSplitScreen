using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RawInputSharp;
using System;
using System.Collections.Generic;

namespace SplitScreen.Mice
{
	//Uses RawInputSharp from http://jstookey.com/arcade/rawmouse/
	//https://github.com/Ilyaki/SplitScreen/blob/master/SplitScreen/Mice/MultipleMiceManager.cs

	public class MultipleMiceManager
	{
		private static RawMouse attachedMouse;
		private static Vector2 totalAttachedDelta = new Vector2(0, 0);
		private static int totalMouseZ = 0;

		private InputDisabler inputDisabler = null;

		private static RawMouseInput rawMouseInput;

		private bool keepCheckingForMouseAttach = false;
		private bool lockMouse = false;

		public static string AttachedMouseID
		{
			get
			{
				if (attachedMouse != null) return attachedMouse.Handle.ToString();
				else return "ANY";
			}
		}

		public static bool HasAttachedMouse() => attachedMouse != null;

		//Vector2 is total delta
		private static List<RawMouse> mice = new List<RawMouse>();

		public MultipleMiceManager()
		{
			//mouseDisabler = new MouseDisabler(); (moved to RegisterMice)
			//RegisterMice();
			Events.QuitGame += delegate { DetachMouse(); };
		}

		#region Registering mice
		private bool hasRegistered = false;
		public void RegisterMice()
		{
			if (hasRegistered) return;
			hasRegistered = true;

			rawMouseInput = new RawMouseInput();
			rawMouseInput.RegisterForWM_INPUT(System.Windows.Forms.Control.FromHandle(Terraria.Main.instance.Window.Handle).FindForm().Handle);//form.handle
			System.Windows.Forms.Application.AddMessageFilter(new PreMessageFilter());

			foreach (object rawMouseObj in rawMouseInput.Mice)
			{
				if (rawMouseObj != null)
					mice.Add((RawMouse)rawMouseObj);
			}
			Console.WriteLine($"Registered mouse driver, found {mice.Count} mice");
		}

		private class PreMessageFilter : System.Windows.Forms.IMessageFilter
		{
			public bool PreFilterMessage(ref System.Windows.Forms.Message m)
			{
				if (m != null && m.Msg == 0x00FF && rawMouseInput != null)
					rawMouseInput.UpdateRawMouse(m.LParam);
				return false;
			}
		}
		#endregion

		public void Update()
		{
			if (keepCheckingForMouseAttach)
			{
				foreach (RawMouse mouse in mice)
				{
					if (attachedMouse != mouse && mouse.Buttons[0])
					{
						attachedMouse = mouse;
						keepCheckingForMouseAttach = false;
						totalAttachedDelta = new Vector2(0, 0);
						Monitor.Log($"Set attached mouse to {mouse.Handle.ToString()}");

						if (lockMouse)
						{
							inputDisabler = inputDisabler ?? new InputDisabler();
							inputDisabler.Lock();
						}

						break;
					}
				}
			}

			if (lockMouse && HasAttachedMouse() && inputDisabler != null && !InputDisabler.IsAutoHotKeyNull)
			{
				Mouse.SetPosition(0, 0);

				System.Windows.Forms.Cursor.Position = new System.Drawing.Point(0, 0);
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(1, 1));
			}

			if (HasAttachedMouse())
			{
				totalAttachedDelta.X = Math.Max(0, Math.Min(totalAttachedDelta.X + attachedMouse.XDelta, Terraria.Main.instance.GraphicsDevice.PresentationParameters.Bounds.Width - 1));
				totalAttachedDelta.Y = Math.Max(0, Math.Min(totalAttachedDelta.Y + attachedMouse.YDelta, Terraria.Main.instance.GraphicsDevice.PresentationParameters.Bounds.Height - 1));
				totalMouseZ += attachedMouse.ZDelta * 5000;
			}

		}

		public void AttachMouseButtonClicked()
		{
			keepCheckingForMouseAttach = true;
		}

		public void LockMouse()
		{
			lockMouse = true;
			//mouseDisabler.Lock();
		}



		private void DetachMouse()
		{
			if (attachedMouse != null)
			{
				attachedMouse = null;
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				inputDisabler?.Unlock();
				lockMouse = false;

				Monitor.Log("Detached mouse");
			}
		}

		public static MouseState? GetAttachedMouseState()
		{
			if (attachedMouse == null)
				return null;

			int leftButtonState = attachedMouse.Buttons[0] ? 1 : 0;
			int rightButtonState = attachedMouse.Buttons[2] ? 1 : 0;
			int middleButtonState = attachedMouse.Buttons[1] ? 1 : 0;
			return new MouseState((int)totalAttachedDelta.X, (int)totalAttachedDelta.Y, totalMouseZ, (ButtonState)leftButtonState, (ButtonState)rightButtonState, (ButtonState)middleButtonState, ButtonState.Released, ButtonState.Released);
		}
	}
}
