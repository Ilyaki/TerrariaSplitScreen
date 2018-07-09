using SplitScreen.Keyboards.SplitScreen.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using Microsoft.Xna.Framework;
using SplitScreen.UI;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;
using SplitScreen.Mice;

namespace SplitScreen.UI
{
	public class UIController
	{
		private GamePadSelectorUI gamePadSelectorUI;
		private AttachMouseUI attachMouseUI;
		private AttachKeyboardUI attachKeyboardUI;
		private LockMouseUI lockMouseUI;
		private UserInterface deviceSelectorInterface;

		private bool mainIsActive = false;

		private MultipleKeyboardManager keyboardManager;
		private MultipleMiceManager miceManager;

		public UIController(MultipleKeyboardManager keyboardManager, MultipleMiceManager miceManager)
		{
			this.keyboardManager = keyboardManager;
			this.miceManager = miceManager;

			deviceSelectorInterface = new UserInterface();

			attachMouseUI = new AttachMouseUI(miceManager);
			//attachMouseUI.Activate();
			attachMouseUI.IsActive = false;
			attachMouseUI.OnDeactivated += delegate
			{
				mainIsActive = false;
			};
			
			lockMouseUI = new LockMouseUI();
			//lockMouseUI.Activate();
			lockMouseUI.IsActive = false;
			lockMouseUI.OnDeactivated += delegate {
				deviceSelectorInterface.SetState(attachMouseUI);
				attachMouseUI.IsActive = true;
				attachMouseUI.ShouldLockMouse = lockMouseUI.ShouldLock;
			};

			attachKeyboardUI = new AttachKeyboardUI(keyboardManager);
			//attachKeyboardUI.Activate();
			attachKeyboardUI.IsActive = false;
			attachKeyboardUI.OnDeactivated += delegate {
				deviceSelectorInterface.SetState(lockMouseUI);
				lockMouseUI.IsActive = true;
			};

			gamePadSelectorUI = new GamePadSelectorUI();
			//gamePadSelectorUI.Activate();
			gamePadSelectorUI.IsActive = false;
			gamePadSelectorUI.OnDeactivated += delegate {
				if (PlayerIndexController.Index == null)
				{
					deviceSelectorInterface.SetState(attachKeyboardUI);
					attachKeyboardUI.IsActive = true;
				}
			};

			deviceSelectorInterface.SetState(gamePadSelectorUI);
			deviceSelectorInterface.IsVisible = false;
			mainIsActive = false;
		}

		public void ActivateUI()
		{
			gamePadSelectorUI.IsActive = true;
			mainIsActive = true;
		}

		public void UpdateUI(GameTime gameTime)
		{
			if (mainIsActive && ((IActiveUI)deviceSelectorInterface?.CurrentState).IsActive)
			{
				((IActiveUI)deviceSelectorInterface?.CurrentState).RecalculatePositions();
				deviceSelectorInterface.Update(gameTime);
			}
		}

		public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			if (mainIsActive && ((IActiveUI)deviceSelectorInterface?.CurrentState).IsActive)
			{
				int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
				if (MouseTextIndex != -1)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"SplitScreen: DeviceSelector",
					delegate
					{
						deviceSelectorInterface.CurrentState.Draw(Terraria.Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
				}
			}
		}
	}
}
