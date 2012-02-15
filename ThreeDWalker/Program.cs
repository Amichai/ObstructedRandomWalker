using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace ThreeDWalker {
	class Program {
		static void Main(string[] args) {
			Walker2 walker = new Walker2();
			foreach (var i in walker.Walk(500000, 1)) {

			}
			walker.PrintHeatMap();
		}
	}
}
