using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;

namespace ThreeDWalker {
	public class Heatmap {
		private int stepsToAssess, magnification, size;
		public Heatmap(int stepsToAssess, int magnification, int size) {
			this.stepsToAssess = stepsToAssess;
			this.magnification = magnification;
			this.size = size * magnification;
			threeDArray = new List<List<List<int>>>(this.size);
			initialize3DArray(this.size);
		}
		private void initialize3DArray(int size) {
			var A = new List<int>(size);
			for (int i = 0; i < size; i++) {
				A.Add(0);
			}
			var B = new List<List<int>>(size);
			for (int i = 0; i < size; i++) {
				B.Add(A);
			}
			for (int i = 0; i < size; i++) {
				threeDArray.Add(B);
			}
		}
		List<List<List<int>>> threeDArray;
		//As paths get added, add to this data set
		private LinkedList<Point> points = new LinkedList<Point>();

		public void AddStep(Point newPoint) {
			if (points.Count() >= stepsToAssess) {
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
			if(Math.Abs(xDistance) >= size/2 || Math.Abs(yDistance)>= size/2 || Math.Abs(zDistance) >= size/2){
				outOfRangeCounter++;
				Debug.Print(outOfRangeCounter.ToString() + " " + xDistance.ToString() + " " + yDistance.ToString() + " " + zDistance.ToString());
			}else
				threeDArray[xDistance + size / 2][yDistance + size / 2][zDistance + size / 2]++;
		}

		internal void Print() {
			int counter = 0;
			foreach( var b in threeDArray.GetBitmaps(this.size, this.size, this.size)){
				b.Save("heatmap" + (++counter).ToString() + ".bmp");
			}
		}
	}
}
