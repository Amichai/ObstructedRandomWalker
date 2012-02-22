using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace ThreeDWalker {
	class Program {
		static void Main(string[] args) {
			var heatmap = new Heatmap(5,20, 5, 10);
			Walker2 walker = new Walker2(heatmap);
			//2000000 is approximately a 30 minute trial
			int lastPercent = 0;
			Debug.Print(lastPercent.ToString());
			foreach (var i in walker.Walk(100000, .05)) {
				if(i.ProgressValue != lastPercent){
					Debug.Print(i.ProgressValue.ToString());
					lastPercent = i.ProgressValue;
				}
			}
			//walker.PrintProjections(100);
			walker.PrintHeatMap();
		}
	}
}
