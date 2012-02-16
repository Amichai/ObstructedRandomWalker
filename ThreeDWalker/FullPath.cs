using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Drawing;

namespace ThreeDWalker {
	public class FullPath {
		double xMax = double.MinValue, yMax = double.MinValue, zMax = double.MinValue;
		double xMin = double.MaxValue, yMin = double.MaxValue, zMin = double.MaxValue;
		public List<Point> fullPath = new List<Point>();
		public void Add(Point pt) {
			if (pt.X > xMax) { xMax = pt.X; }
			if (pt.Y > yMax) { yMax = pt.Y; }
			if (pt.Z > zMax) { zMax = pt.Z; }
			if (pt.X < xMin) { xMin = pt.X; }
			if (pt.Y < yMin) { yMin = pt.Y; }
			if (pt.Z < zMin) { zMin = pt.Z; }
			fullPath.Add(pt);
		}
		public int Count() { return fullPath.Count(); }

		public IEnumerable<Point> GetData() {
			return fullPath.AsEnumerable();
		}

		public void PrintProjections(int magnification) {
			xMin *= magnification; yMin *= magnification; zMin *= magnification;
			xMax *= magnification; yMax *= magnification; zMax *= magnification;
			double width = (xMax - xMin);
			double height = (yMax - yMin);
			double depth = (zMax - zMin);
			Bitmap xyProj = new Bitmap((int)width + 1, (int)height + 1);
			Bitmap xzProj = new Bitmap((int)width + 1, (int)depth + 1);
			Bitmap yzProj = new Bitmap((int)height + 1, (int)depth + 1);
			for (int i = 0; i < fullPath.Count(); i++) {
				var pt = fullPath[i] * magnification;
				xyProj.SetPixel((int)(pt.X - xMin), (int)(pt.Y - yMin), System.Drawing.Color.Black);
				xzProj.SetPixel((int)(pt.X - xMin), (int)(pt.Z - zMin), System.Drawing.Color.Black);
				yzProj.SetPixel((int)(pt.Y - yMin), (int)(pt.Z - zMin), System.Drawing.Color.Black);
			}
			string filepath = @"c:\users\amichai\documents\visual studio 2010\projects\randomwalker\threedwalker\bin\debug\";

			xyProj.Save(filepath + "XYProjection.bmp");
			xzProj.Save(filepath + "XZProjection.bmp");
			yzProj.Save(filepath + "YZProjection.bmp");
		}
	}
}
