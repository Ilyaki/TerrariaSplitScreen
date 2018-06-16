using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SplitScreen
{
	class PlayerIndexController
	{
		private static GamePadState blankState = new GamePadState(new Vector2(), new Vector2(), 0,0);

		public static PlayerIndex? Index { get; private set; } = PlayerIndex.One;//One as default means controllers will work in single player/main menu

		public static void IncrementPlayerIndex()
		{
			switch (Index)
			{
				case PlayerIndex.One:
				case PlayerIndex.Two:
				case PlayerIndex.Three:
					Index = (PlayerIndex)(Index + 1); break;
				case PlayerIndex.Four:
					Index = null; break;
				default:
					Index = PlayerIndex.One; break;
			}
		}

		public static GamePadState GetGamepadState()
		{
			if (Index.HasValue) return GamePad.GetState((PlayerIndex)Index);
			else return blankState;
		}
	}
}
