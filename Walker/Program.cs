using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Walker {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Map mapToRender = Map.MapBuilder();
			Application.Run(new WalkerForm(mapToRender));
		}
	}
}
