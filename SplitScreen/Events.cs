using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitScreen
{
	public class Events
	{
		public Events()
		{
			Terraria.Main.instance.Exiting += (o,e) => OnQuitGame(null);//Alt+F4
		}

		public static event EventHandler QuitGame;
		public void OnQuitGame(EventArgs e) => Events.QuitGame?.Invoke(this, e);

		public static event EventHandler PreUpdate;
		public void OnPreUpdate(EventArgs e) => Events.PreUpdate?.Invoke(this, e);

		/// <summary>
		/// Runs on the Terraria main game thread
		/// </summary>
		public static event EventHandler SecondaryLoad;
		public void OnSecondaryLoad(EventArgs e) => Events.SecondaryLoad?.Invoke(this, e);
	}
}
