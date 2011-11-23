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
		List<LineSegment> pathData; 
		ThreeDimArray heatMap;
		int numberOfStepsToCompute, boardSize, precisionCoef;
		public Heatmap(List<LineSegment> pathData, int stepsToComp, int boardSize, int precisionCoef = 2){
			this.pathData = pathData;
			this.numberOfStepsToCompute = stepsToComp;
			this.boardSize = boardSize;
			this.heatMap = new ThreeDimArray(numberOfStepsToCompute, boardSize);
			this.precisionCoef = precisionCoef;
		}
		//As paths get added, add to this data set
		public void AddPath(LineSegment path) {
			//TODO: implement me
		}

		//Handle all path data at once
		//Implement schabse's reporter class
		public void BuildHeatMap(){
			for (int i = 0; i < pathData.Count() - numberOfStepsToCompute; i++) {
				var line = pathData[i];
				for (int j = 0; j <numberOfStepsToCompute; j++) {
					var endPoint = pathData[i + j];
					int xDistance = (int)Math.Round(endPoint.EndingPos.GetX() * precisionCoef) - (int)Math.Round(line.StartingPos.GetX() * precisionCoef);
					int yDistance = (int)Math.Round(endPoint.EndingPos.GetY() * precisionCoef) - (int)Math.Round(line.StartingPos.GetY() * precisionCoef);
					heatMap.IncrementPixel(xDistance,yDistance, j);
				}
			}
		}
		public void Print() {
			heatMap.SaveToDisk();
		}
	}
}
