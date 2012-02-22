using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;
using System.Drawing;

namespace ThreeDWalker {
	public class Heatmap {
		private int stepsToAssess, magnification, size, nt, ns;
		/// <param name="ns">this is the elementary time step between measurements</param>
		/// <param name="nt">gives the number of measurements to take as ns*ns*k*k as k goes from 1 to nt</param>
		public Heatmap(int ns, int nt, int magnification, int size) {
			this.stepsToAssess = ns * ns * nt * nt;
			this.nt = nt;
			this.ns = ns;
			this.magnification = magnification;
			this.size = size * magnification;
			for (int i = 0; i < nt; i++) {
				var a = new int[this.size, this.size, this.size];
				threeDArrays.Add(a);
			}
		}
		List<int[, ,]> threeDArrays = new List<int[, ,]>();
		//As paths get added, add to this data set
		private LinkedList<Point> points = new LinkedList<Point>();

		public void AddStep(Point newPoint) {
			if (points.Count() >= stepsToAssess) {
				for(int i=0; i< this.nt; i++){
					int stepToSave = i * i * this.ns * this.ns;
					incrementHeatMapValues(points.ElementAt(stepToSave), newPoint, i);
				}
				points.RemoveFirst();
			}
			points.AddLast(newPoint);
			if(points.Count() > this.stepsToAssess)
				throw new Exception("The rolling cache is too big");
		}

		int outOfRangeCounter = 0;
		private void incrementHeatMapValues(Point startPt, Point endPt, int idx) {
			int xDistance = (int)Math.Round(endPt.X * magnification - startPt.X * magnification);
			int yDistance = (int)Math.Round(endPt.Y * magnification - startPt.Y * magnification);
			int zDistance = (int)Math.Round(endPt.Z * magnification - startPt.Z * magnification);
			if (Math.Abs(xDistance) >= size / 2 || Math.Abs(yDistance) >= size / 2 || Math.Abs(zDistance) >= size / 2) {
				outOfRangeCounter++;
			} else {
				int xIdx = xDistance + size / 2, yIdx = yDistance + size / 2, zIdx = zDistance + size / 2;
				threeDArrays[idx][xIdx,yIdx,zIdx]++;
			}
		}

		internal void Print() {
			for (int idx = nt - 1; idx >= 0; idx--) {
				int counter = 0;
				for (int i = 0; i < this.size; i++) {
					int[,] matrixToPrint = new int[this.size, this.size];
					for (int j = 0; j < this.size; j++) {
						for (int k = 0; k < this.size; k++) {
							matrixToPrint[j, k] = threeDArrays[idx][j, k, i];
						}
					}
					var B = matrixToPrint.ConvertToBitmap(Color.White);
					B.Save(@"c:\users\amichai\documents\visual studio 2010\projects\randomwalker\threedwalker\bin\debug\heatmap" + (nt - idx).ToString() + "steps" + (++counter).ToString() + ".bmp");
				}
			}
		}
	}
}
