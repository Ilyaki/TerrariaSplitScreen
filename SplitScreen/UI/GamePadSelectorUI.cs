using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SplitScreen.UI;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SplitScreen
{
	class GamePadSelectorUI : UIState, IActiveUI
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
		const int panelWidth = 400;
		const int panelHeight = 120;

		private UITextPanel<string> changeGamepadButton;

		private UIText playerIndexDisplay;

		private UITextPanel<string> finishButton;


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

			//Change gamepad button
			changeGamepadButton = new UITextPanel<string>("Next", 0.5f, true)
			{
				VAlign = 0.05f,
				HAlign = 0.03f
			};
			changeGamepadButton.OnClick += new UIElement.MouseEvent(this.ChangedButtonClicked);
			backPanel.Append(changeGamepadButton);

			//Player index display
			playerIndexDisplay = new UIText("", 1.15f, false)
			{
				VAlign = 0.07f,
				HAlign = 0.6f
			};
			backPanel.Append(playerIndexDisplay);

			//Finished button
			finishButton = new UITextPanel<string>("OK", 0.5f, true)
			{
				VAlign = 0.9f,
				HAlign = 0.9f
			};
			finishButton.OnClick += FinishButtonClicked;
			backPanel.Append(finishButton);
		}

		private void FinishButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Terraria.Main.PlaySound(10);
			IsActive = false;
		}

		private void ChangedButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Terraria.Main.PlaySound(10);
			PlayerIndexController.IncrementPlayerIndex();
			Monitor.Log($"Switched player index to {PlayerIndexController.Index?.ToString() ?? "NONE"}");
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			playerIndexDisplay.SetText($"Current gamepad index = {PlayerIndexController.Index?.ToString().ToUpper() ?? "NONE"}");

			Vector2 MousePosition = new Vector2((float)Terraria.Main.mouseX, (float)Terraria.Main.mouseY);
			if (backPanel.ContainsPoint(MousePosition))
			{
				Terraria.Main.LocalPlayer.mouseInterface = true;
			}

			base.DrawSelf(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			Terraria.Main.blockInput = IsActive;
			base.Update(gameTime);
		}
	}
}
