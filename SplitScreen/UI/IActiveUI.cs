using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitScreen.UI
{
	public interface IActiveUI
	{
		bool IsActive { get; set; }

		void RecalculatePositions();
	}
}
