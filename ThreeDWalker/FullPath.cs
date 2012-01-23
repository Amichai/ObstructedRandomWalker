using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ThreeDWalker {
	public class FullPath {
		public List<Point> fullPath = new List<Point>();
		public void Add(Point pt) {
			fullPath.Add(pt);
		}
		public int Count() { return fullPath.Count(); }

		public IEnumerable<Point> GetData() {
			return fullPath.AsEnumerable();
		}
	}
}
