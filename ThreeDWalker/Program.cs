using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeDWalker {
	class Program {
		static void Main(string[] args) {
			ThreeDWalker walker = new ThreeDWalker();
			walker.Walk(10000, 1);
		}
	}
}
