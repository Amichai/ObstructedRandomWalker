using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ThreeDWalker {
	public class Heatmap {
		private int stepsToAssess, magnification, size;
		public Heatmap(int stepsToAssess, int magnification, int size) {
			this.stepsToAssess = stepsToAssess;
			this.magnification = magnification;
			this.size = size * magnification;
			threeDArray = new List<List<List<int>>>(this.size);
		}
		List<List<List<int>>> threeDArray;
		//As paths get added, add to this data set
		private LinkedList<Point> points = new LinkedList<Point>();

		public void AddStep(Point newPoint) {
			if (points.Count() > stepsToAssess) {
				incrementHeatMapValues(points.First(), newPoint);
				points.RemoveFirst();
			}
			points.AddLast(newPoint);
			if(points.Count() > this.stepsToAssess)
				throw new Exception("The rolling cache is too big");
		}

		private void incrementHeatMapValues(Point startPt, Point endPt) {
			int outOfRangeCounter = 0;
			int xDistance = (int)Math.Round(endPt.X * magnification) - (int)Math.Round(startPt.X * magnification);
			int yDistance = (int)Math.Round(endPt.Y * magnification) - (int)Math.Round(startPt.Y * magnification);
			int zDistance = (int)Math.Round(endPt.Z * magnification) - (int)Math.Round(startPt.Z * magnification);
			if(xDistance > size || yDistance> size || zDistance > size){
				outOfRangeCounter++;
				Debug.Print(outOfRangeCounter.ToString() + " " + xDistance.ToString() + " " + yDistance.ToString() + " " + zDistance.ToString());
			}else
				threeDArray[xDistance][yDistance][zDistance]++;
		}
	}
}
