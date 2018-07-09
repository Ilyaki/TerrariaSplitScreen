using Harmony;
using Microsoft.Xna.Framework.Input;
using SplitScreen.Mice;
using System;

namespace SplitScreen
{
	//Usually ignores mouse when window is inactive (Don't use Main.IsActive, that is always set to true)

	[HarmonyPatch(typeof(Microsoft.Xna.Framework.Input.Mouse))]
	[HarmonyPatch("GetState")]
	[HarmonyPatch(new Type[] { })]
	public class Mouse_GetState_Patcher
	{
		public static MouseState Postfix(MouseState m, MouseState __result)
		{
			if (MultipleMiceManager.HasAttachedMouse() || !Utility.TrueIsWindowActive())
				return ((Utility.IsMouseLocked() && MultipleMiceManager.HasAttachedMouse()) ? MultipleMiceManager.GetAttachedMouseState() : null) ?? default(MouseState);
			else return __result;
		}
	}
}
