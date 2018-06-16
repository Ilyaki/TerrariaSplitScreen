using Harmony;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;

namespace SplitScreen
{
	public class SplitScreenMod : Mod
	{
		private DeviceSelectorUI deviceSelectorUI;
		private UserInterface deviceSelectorInterface;

		public override void Load()
		{
			Monitor.Log("Loading...");
			
			//Disables FPS throttling when window is unfocused
			Terraria.Main.instance.InactiveSleepTime = new TimeSpan(0);

			HarmonyInstance harmony = HarmonyInstance.Create("me.ilyaki.terrariaSplitScreen");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			//UI
			deviceSelectorUI = new DeviceSelectorUI();
			deviceSelectorUI.Activate();
			deviceSelectorUI.IsActive = false;
			deviceSelectorInterface = new UserInterface();
			deviceSelectorInterface.SetState(deviceSelectorUI);

			Terraria.Main.OnTick += OnTick;
			
			base.Load();
		}

		private bool oldIsConnected = false;
		private void OnTick()
		{
			if (!oldIsConnected && Utility.IsConnectedToAServer())
			{
				Monitor.Log("Activating UI");
				deviceSelectorUI.IsActive = true;
				Terraria.Main.blockInput = true;
			}
			oldIsConnected = Utility.IsConnectedToAServer();
		}
		
		public override void UpdateUI(GameTime gameTime)
		{
			if (deviceSelectorUI.IsActive) deviceSelectorInterface.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			if (deviceSelectorUI.IsActive)
			{
				int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
				if (MouseTextIndex != -1)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"SplitScreen: DeviceSelector",
					delegate
					{
						deviceSelectorUI.Draw(Terraria.Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
				}
			}
		}
	}
}
