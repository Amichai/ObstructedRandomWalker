using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;


namespace Walker {
	/// <summary>Takes path data and renders n density maps where n is the number of steps taken from the origin.
	/// Wraps the ThreeDMatrix Class. </summary>
	class Heatmap {
		List<LineSegment> pathData =null; 
		ThreeDimArray threeDArray;
		int numberOfStepsToCompute = 60;
		int boardSize, precisionCoef;

		public Heatmap(int stepSize, int stepsToCompute, int boardSize, int precisionCoef = 2, List<LineSegment> pathData = null) {
			this.pathData = pathData;
			this.numberOfStepsToCompute = stepsToCompute;
			this.boardSize = boardSize;
			this.threeDArray = new ThreeDimArray(numberOfStepsToCompute, boardSize, stepSize*precisionCoef);
			this.precisionCoef = precisionCoef;
			rollingCache = new LinkedList<LineSegment>();
		}
		//As paths get added, add to this data set
		public LinkedList<LineSegment> rollingCache;

		public void AddPath(LineSegment path) {
			if (rollingCache.Count() == numberOfStepsToCompute) {
				for (int i = 1; i < numberOfStepsToCompute; i++) {
					incrementHeatMapValues(rollingCache.First(), rollingCache.ElementAt(i), i - 1);
				}
				rollingCache.RemoveFirst();
			}
			rollingCache.AddLast(path);
			if (rollingCache.Count() > numberOfStepsToCompute)
				throw new Exception("The rolling cache is too big");
		}

		private void incrementHeatMapValues(LineSegment startLine, LineSegment endLine, int stepIdx) {
			int xDistance = (int)Math.Round(endLine.StartingPos.GetX() * precisionCoef) - (int)Math.Round(startLine.EndingPos.GetX() * precisionCoef);
			int yDistance = (int)Math.Round(endLine.StartingPos.GetY() * precisionCoef) - (int)Math.Round(startLine.EndingPos.GetY() * precisionCoef);
			threeDArray.IncrementPixel(xDistance, yDistance, stepIdx);
		}


		/// <summary>
		/// This handle all path data at once. Assumes all path data was passed to the constructor. 
		/// </summary>
		public void BuildHeatMap(){
			for (int i = 0; i < pathData.Count() - numberOfStepsToCompute; i++) {
				var startLine = pathData[i];
				for (int j = 0; j <numberOfStepsToCompute; j++) {
					var endLine = pathData[i + j];
					incrementHeatMapValues(startLine, endLine, j);
				}
			}
		}

		public void Print() {
			threeDArray.SaveToDisk();
		}
	}
}
