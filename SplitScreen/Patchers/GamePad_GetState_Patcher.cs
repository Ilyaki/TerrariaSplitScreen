using Harmony;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace SplitScreen
{
	[HarmonyPatch(typeof(Microsoft.Xna.Framework.Input.GamePad))]
	[HarmonyPatch("GetState")]
	[HarmonyPatch(new Type[] { typeof(PlayerIndex), typeof(GamePadDeadZone) })]
	public class GamePad_GetState_Patcher
	{
		public static GamePadState Postfix(GamePadState g, PlayerIndex playerIndex, GamePadState __result)
		{
			if (playerIndex.Equals(PlayerIndex.One) && PlayerIndexController.Index != PlayerIndex.One)
				return PlayerIndexController.GetGamepadState();
			else return __result;
		}
	}
}
