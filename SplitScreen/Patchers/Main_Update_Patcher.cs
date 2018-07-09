using Harmony;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SplitScreen
{
	[HarmonyPatch(typeof(Terraria.Main))]
	[HarmonyPatch("DoUpdate")]
	[HarmonyPatch(new Type[] { typeof(GameTime) })]
	public class Main_Update_Patcher
	{
		private static bool hasSecondaryLoaded = false;

		public static bool Prefix(GameTime gameTime)
		{
			if (Utility.IsConnectedToAServer())
			{
				Terraria.Main.instance.SetPrivateFieldValue("isActive", true);
				Terraria.Main.hasFocus = true;
				Terraria.Main.gameInactive = false;
			}

			SplitScreenMod.Events.OnPreUpdate(null);

			if (!hasSecondaryLoaded)
			{
				hasSecondaryLoaded = true;
				SplitScreenMod.Events.OnSecondaryLoad(null);
			}

			return true;
		}

		/*This makes it so in Main.DoUpdate, Main.hasFocus isn't set to false when Form.active isn't current window
		It will only care about Main.IsActive (XNA.Game.IsActive)

		c#
		...
		Main.hasFocus = this.IsActive;
		(SNIP THIS LINE): Main.hasFocus = Form.ActiveForm == Control.FromHandle(this.Window.Handle) as Form;
		if (!Main.gameMenu || Main.netMode == 2)
		{
		...

		IL
		...
		IL_0688: ldarg.0
		IL_0689: call instance bool [Microsoft.Xna.Framework.Game]Microsoft.Xna.Framework.Game::get_IsActive()
		IL_068e: stsfld bool Terraria.Main::hasFocus
		(SNIP START) IL_0693: call class [System.Windows.Forms]System.Windows.Forms.Form [System.Windows.Forms]System.Windows.Forms.Form::get_ActiveForm()
		IL_0698: ldarg.0
		IL_0699: call instance class [Microsoft.Xna.Framework.Game]Microsoft.Xna.Framework.GameWindow [Microsoft.Xna.Framework.Game]Microsoft.Xna.Framework.Game::get_Window()
		IL_069e: callvirt instance native int [Microsoft.Xna.Framework.Game]Microsoft.Xna.Framework.GameWindow::get_Handle()
		IL_06a3: call class [System.Windows.Forms]System.Windows.Forms.Control [System.Windows.Forms]System.Windows.Forms.Control::FromHandle(native int)
		IL_06a8: isinst [System.Windows.Forms]System.Windows.Forms.Form
		IL_06ad: ceq
		IL_06af: stsfld bool Terraria.Main::hasFocus (SNIP END)
		IL_06b4: ldsfld bool Terraria.Main::gameMenu
		IL_06b9: brfalse.s IL_06c3
		...

		*/
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			Monitor.Log("Performing edit...");

			var codes = new List<CodeInstruction>(instructions);

			int startIndex = -1;
			int endIndex = -1;

			for (int i = 0; i < codes.Count; i++)
			{
				try
				{
					bool a = (codes[i].operand.ToString()).EndsWith("get_ActiveForm()");
					bool b = (codes[i + 2].operand.ToString()).EndsWith("get_Window()");
					bool c = (codes[i + 7].operand.ToString()).EndsWith("hasFocus");
					bool d = codes[i + 7].opcode == OpCodes.Stsfld;

					if (a && b && c && d)
					{
						startIndex = i;
						endIndex = i + 7;
						break;
					}
				}
				catch (Exception) { }
			}

			if (startIndex != -1 && endIndex != -1)
			{
				Monitor.Log($"Cutting between {startIndex} and {endIndex}");

				for (int i = startIndex; i <= endIndex; i++)
					codes[i].opcode = OpCodes.Nop;
			}
			else
			{
				Monitor.Log("Couldn't find target code to edit!");
			}
			
			return codes.AsEnumerable();
		}
	}
}
