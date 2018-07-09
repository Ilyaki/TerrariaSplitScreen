using System;

namespace SplitScreen
{
	class ToggleBorders
	{
		private bool oldIsBorderlessKeyPressed;
		private bool isBorderless = false;

		private const Microsoft.Xna.Framework.Input.Keys key = Microsoft.Xna.Framework.Input.Keys.F6;
		
		private void Flip()
		{
			try
			{
				var form = Utility.GetForm();
				if (form != null)
				{					
					Terraria.Main.graphics.PreferredBackBufferHeight += isBorderless ? -31 : 32;
					Terraria.Main.graphics.PreferredBackBufferWidth += isBorderless ? -2 : 2;
					Terraria.Main.SetDisplayMode(Terraria.Main.graphics.PreferredBackBufferWidth, Terraria.Main.graphics.PreferredBackBufferHeight, false);
					form.Location = new System.Drawing.Point(form.Location.X + 7 * (isBorderless ? -1 : 1), form.Location.Y);

					Terraria.Main.screenBorderless = !isBorderless;
					form.FormBorderStyle = isBorderless ? System.Windows.Forms.FormBorderStyle.Sizable : System.Windows.Forms.FormBorderStyle.None;

					isBorderless = !isBorderless;
				}
			}
			catch (Exception) { }
		}

		public void Update()
		{
			//Borderless
			var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

			if (!oldIsBorderlessKeyPressed && keyboardState.IsKeyDown(key))
				Flip();

			oldIsBorderlessKeyPressed = keyboardState.IsKeyDown(key);
		}
	}
}
