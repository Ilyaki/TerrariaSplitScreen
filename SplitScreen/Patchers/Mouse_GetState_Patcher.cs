using Harmony;
using Microsoft.Xna.Framework.Input;
using System;

namespace SplitScreen
{
	//Ignores mouse when window is inactive (Don't use Main.IsActive, that is always set to true)

	[HarmonyPatch(typeof(Microsoft.Xna.Framework.Input.Mouse))]
	[HarmonyPatch("GetState")]
	[HarmonyPatch(new Type[] { })]
	public class Mouse_GetState_Patcher
	{
		public static MouseState Postfix(MouseState m, MouseState __result)
		{
			if (!Utility.TrueIsWindowActive())
				return default(MouseState);
			else return __result;
		}
	}
}
