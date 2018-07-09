using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SplitScreen.Keyboards.SplitScreen.Keyboards;
using SplitScreen.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SplitScreen.UI
{
	class AttachKeyboardUI : UIState, IActiveUI
	{
		private bool isActive = true;
		public bool IsActive
		{
			get { return isActive; }
			set
			{
				if (isActive && !value)
					OnDeactivated?.Invoke(this, null);

				isActive = value;
			}
		}
		public event EventHandler OnDeactivated;


		private UIPanel backPanel;
		const int panelWidth = 350;
		const int panelHeight = 44;

		private UIText finishButton;

		private MultipleKeyboardManager keyboardManager;

		public AttachKeyboardUI(MultipleKeyboardManager keyboardManager)
		{
			this.keyboardManager = keyboardManager;
		}

		public void RecalculatePositions()
		{
			backPanel.Left.Set(Terraria.Main.instance.GraphicsDevice.Viewport.Bounds.Width / 2 - panelWidth / 2, 0f);
			backPanel.Top.Set(Terraria.Main.instance.GraphicsDevice.Viewport.Bounds.Height / 2 - panelHeight / 2, 0f);
		}

		public override void OnInitialize()
		{
			//Panel
			backPanel = new UIPanel();
			backPanel.SetPadding(0);
			backPanel.Left.Set(Terraria.Main.instance.GraphicsDevice.Viewport.Bounds.Width / 2 - panelWidth / 2, 0f);
			backPanel.Top.Set(Terraria.Main.instance.GraphicsDevice.Viewport.Bounds.Height / 2 - panelHeight / 2, 0f);
			backPanel.Width.Set(panelWidth, 0f);
			backPanel.Height.Set(panelHeight, 0f);
			base.Append(backPanel);

			//Finished button
			
			finishButton = new UIText("Press any 3 keys", 0.7f, true)
			{
				VAlign = 0.21f,
				HAlign = 0.15f
			};

			backPanel.Append(finishButton);
		}

		public override void Update(GameTime gameTime)
		{
			if (keyboardManager.CheckKeyboardsToAttach())
			{
				Monitor.Log("Attached keyboard");
				Terraria.Main.PlaySound(10);
				IsActive = false;
			}

			Terraria.Main.blockInput = IsActive;

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 MousePosition = new Vector2((float)Terraria.Main.mouseX, (float)Terraria.Main.mouseY);
			if (backPanel.ContainsPoint(MousePosition))
			{
				Terraria.Main.LocalPlayer.mouseInterface = true;
			}

			base.DrawSelf(spriteBatch);
		}
	}
}
