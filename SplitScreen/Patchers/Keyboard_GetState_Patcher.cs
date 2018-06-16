using Harmony;
using Microsoft.Xna.Framework.Input;
using System;

namespace SplitScreen.Patchers
{
	//Ignores keyboard when window is inactive (Don't use Main.IsActive, that is always set to true)

	[HarmonyPatch(typeof(Microsoft.Xna.Framework.Input.Keyboard))]
	[HarmonyPatch("GetState")]
	[HarmonyPatch(new Type[] { typeof(Microsoft.Xna.Framework.PlayerIndex) })]
	public class Keyboard_GetState_Patcher
	{
		public static KeyboardState Postfix(KeyboardState m, KeyboardState __result)
		{
			if (!Utility.TrueIsWindowActive())
				return default(KeyboardState);
			else return __result;
		}
	}
}
