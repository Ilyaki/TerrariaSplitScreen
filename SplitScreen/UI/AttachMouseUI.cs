using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SplitScreen.Mice;
using SplitScreen.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SplitScreen
{
	class AttachMouseUI : UIState, IActiveUI
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

		public bool ShouldLockMouse { get; set; } = false;

		private UIPanel backPanel;
		const int panelWidth = 335;
		const int panelHeight = 56;

		private UITextPanel<string> finishButton;

		private MultipleMiceManager miceManager;

		public AttachMouseUI(MultipleMiceManager miceManager)
		{
			this.miceManager = miceManager;
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
			finishButton = new UITextPanel<string>("Click to attach mouse", 0.7f, true)
			{
				VAlign = 0.12f,
				HAlign = 0.12f
			};
			finishButton.OnClick += FinishButtonClicked;
			backPanel.Append(finishButton);
		}

		public override void Update(GameTime gameTime)
		{
			Terraria.Main.blockInput = IsActive;
			base.Update(gameTime);
		}

		private void FinishButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Monitor.Log($"Attaching mouse, ShouldLock={ShouldLockMouse}");

			//miceManager.RegisterMice();
			miceManager.AttachMouseButtonClicked();


			if (ShouldLockMouse)
				miceManager.LockMouse();
			
			Terraria.Main.PlaySound(10);

			Monitor.Log("Please click again", LogLevel.ChatBox);

			IsActive = false;
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
