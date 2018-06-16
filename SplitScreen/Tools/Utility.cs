using System.Windows.Forms;

namespace SplitScreen
{
	public static class Utility
	{
		public static bool IsConnectedToAServer() => Terraria.Netplay.Connection.IsActive && !Terraria.Netplay.disconnect;
		public static bool TrueIsWindowActive() => Form.ActiveForm == Control.FromHandle(Terraria.Main.instance.Window.Handle) as Form;
	}
}
