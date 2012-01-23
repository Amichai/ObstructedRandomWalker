using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeDWalker {
	internal class FullPath {
		List<Point> fullPath { get; set; }
		public void Add(Point pt) {
			fullPath.Add(pt);
		}
	}
}
