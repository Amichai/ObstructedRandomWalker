using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using System.Drawing;

namespace ThreeDWalker {
	public class Heatmap {
		private int stepsToAssess, magnification, size;
		public Heatmap(int stepsToAssess, int magnification, int size) {
			this.stepsToAssess = stepsToAssess;
			this.magnification = magnification;
			this.size = size * magnification;
			threeDArray = new int[this.size, this.size, this.size];
		}
		int[, ,] threeDArray;
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

		int outOfRangeCounter = 0;
		private void incrementHeatMapValues(Point startPt, Point endPt) {
			int xDistance = (int)Math.Round(endPt.X * magnification - startPt.X * magnification);
			int yDistance = (int)Math.Round(endPt.Y * magnification - startPt.Y * magnification);
			int zDistance = (int)Math.Round(endPt.Z * magnification - startPt.Z * magnification);
			if (Math.Abs(xDistance) >= size / 2 || Math.Abs(yDistance) >= size / 2 || Math.Abs(zDistance) >= size / 2) {
				outOfRangeCounter++;
				//Debug.Print(outOfRangeCounter.ToString() + " " + xDistance.ToString() + " " + yDistance.ToString() + " " + zDistance.ToString());
			} else {
				int xIdx = xDistance + size / 2, yIdx = yDistance + size / 2, zIdx = zDistance + size / 2;
				threeDArray[xIdx,yIdx,zIdx]++;
				Debug.Print(xIdx.ToString() + "," + yIdx.ToString() + "," + zIdx.ToString());
			}
		}

		internal void Print() {
			int counter = 0;
			for (int i = 0; i < this.size; i++) {
				int[,] matrixToPrint = new int[this.size, this.size];
				for (int j = 0; j < this.size; j++) {
					for (int k = 0; k < this.size; k++) {
						matrixToPrint[j, k] = threeDArray[i, j, k];
					}
				}
				var B = matrixToPrint.ConvertToBitmap(Color.White);
				//var B = threeDArray[i].ConvertDoubleArrayToBitmap(Color.White);
				B.Save(@"c:\users\amichai\documents\visual studio 2010\projects\randomwalker\threedwalker\bin\debug\heatmap" + (++counter).ToString() + ".bmp");
			}
		}
	}
}
