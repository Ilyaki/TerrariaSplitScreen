using System.Windows.Forms;

namespace SplitScreen
{
	public static class Utility
	{
		public static bool IsConnectedToAServer() => Terraria.Netplay.Connection.IsActive && !Terraria.Netplay.disconnect;
		public static bool TrueIsWindowActive() => Form.ActiveForm == GetForm();
		public static Form GetForm() => Control.FromHandle(Terraria.Main.instance.Window.Handle) as Form;
		public static bool IsMouseLocked() => Cursor.Position.X == 0 && Cursor.Position.Y == 0;
	}
}
