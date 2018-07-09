using Harmony;
using Microsoft.Xna.Framework;
using SplitScreen.Keyboards.SplitScreen.Keyboards;
using SplitScreen.Mice;
using SplitScreen.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;

namespace SplitScreen
{
	public class SplitScreenMod : Mod
	{
		public static Events Events { get; private set; }

		private MultipleKeyboardManager keyboardManager;
		private MultipleMiceManager miceManager;

		private UIController uiController;

		private ToggleBorders toggleBorders;

		public override void Load()
		{
			Monitor.Log("Loading...");

			Events = new Events();

			//Disables FPS throttling when window is unfocused
			Terraria.Main.instance.InactiveSleepTime = new TimeSpan(0);

			//Make sure it doesnt start in borderless fullscreen
			Terraria.Main.screenBorderless = false;
			SplitScreen.Events.QuitGame += (o, e) => Terraria.Main.screenBorderless = false;
			toggleBorders = new ToggleBorders();
			
			SplitScreen.Events.SecondaryLoad += delegate {
				//Multiple keyboard/mice
				keyboardManager = new MultipleKeyboardManager();
				miceManager = new MultipleMiceManager();
				miceManager.RegisterMice();

				uiController = new UIController(keyboardManager, miceManager);
				
				Events.PreUpdate += OnPreUpdate;
			};

			try {
				HarmonyInstance harmony = HarmonyInstance.Create("me.ilyaki.terrariaSplitScreen");//Run AFTER subscribing to Events
				harmony.PatchAll(Assembly.GetExecutingAssembly());
			}catch (Exception) {
				Monitor.Log("Could not patch with Harmony. This is probably because of a mod reload instead of a fresh restart");
			}
		}
		
		private bool oldIsConnected = false;
		private void OnPreUpdate(object sender, EventArgs args)
		{
			if (!oldIsConnected && Utility.IsConnectedToAServer())
				uiController.ActivateUI();
			oldIsConnected = Utility.IsConnectedToAServer();

			toggleBorders.Update();

			miceManager.Update();
		}
		
		public override void UpdateUI(GameTime gameTime) => uiController?.UpdateUI(gameTime);

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => uiController?.ModifyInterfaceLayers(layers);
	}
}
