using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SplitScreen.Mice;
using SplitScreen.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SplitScreen.UI 
{
	class LockMouseUI : UIState, IActiveUI
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

		public bool ShouldLock { get; private set; } = false;

		private UIPanel backPanel;
		const int panelWidth = 280;
		const int panelHeight = 100;

		private UIText lockMouseText;
		private UITextPanel<string> yesButton;
		private UITextPanel<string> noButton;

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

			//Text
			lockMouseText = new UIText("Lock mouse?", 1.15f, false)
			{
				VAlign = 0.07f,
				HAlign = 0.07f
			};
			backPanel.Append(lockMouseText);

			//Yes button
			yesButton = new UITextPanel<string>("Yes", 0.7f, true)
			{
				VAlign = 0.85f,
				HAlign = 0.06f
			};
			yesButton.OnClick += YesButtonClicked;
			backPanel.Append(yesButton);

			//No button
			noButton = new UITextPanel<string>("No", 0.7f, true)
			{
				VAlign = 0.85f,
				HAlign = 0.7f
			};
			noButton.OnClick += NoButtonClicked;
			backPanel.Append(noButton);
		}

		private void YesButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			
			Monitor.Log("Locking mouse");
			ShouldLock = true;
			Terraria.Main.PlaySound(10);
			IsActive = false;
		}

		private void NoButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Monitor.Log("Not locking mouse");
			ShouldLock = false;
			Terraria.Main.PlaySound(10);
			IsActive = false;
		}

		public override void Update(GameTime gameTime)
		{
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
