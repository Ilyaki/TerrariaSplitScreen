using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitScreen.Patchers
{
	//Quit through main menu
	[HarmonyPatch(typeof(Terraria.Main))]
	[HarmonyPatch("Exit")]
	[HarmonyPatch(new Type[] { })]
	class Main_QuitGame_Patcher
	{
		public static bool Prefix()
		{
			SplitScreenMod.Events.OnQuitGame(null);
			return true;
		}
	}
}
