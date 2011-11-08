using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;


namespace Walker {
	class Heatmap {
		//Takes a list of path data and renders a density heat map

		//For each point reached by the random walker
		//Increment a counter in a discrete matrix representation
		//n 2d matricies, one for each position after n moves
		List<LineSegment> pathData; ThreeDimensionalMatrix heatMap;
		int numberOfStepsToCompute, boardSize;
		public Heatmap(List<LineSegment> pathData, int stepsToComp = 10, int boardSize = 50){
			this.pathData = pathData;
			this.numberOfStepsToCompute = stepsToComp;
			this.boardSize = boardSize;
			this.heatMap = new ThreeDimensionalMatrix(numberOfStepsToCompute, boardSize);
			
			for (int i = 0; i < pathData.Count() - numberOfStepsToCompute; i++) {
				var line = pathData[i];
				for (int j = 0; j <numberOfStepsToCompute; j++) {
					var endPoint = pathData[i + j];
					int xDistance = (int)Math.Round(endPoint.EndingPos.GetX()*5) - (int)Math.Round(line.StartingPos.GetX()*5);
					int yDistance = (int)Math.Round(endPoint.EndingPos.GetY()*5) - (int)Math.Round(line.StartingPos.GetY()*5);
					heatMap.IncrementPixel(xDistance,yDistance, j);
				}
			}
		}
		public void Print() {
			heatMap.SaveToDisk();
		}
	}

	class ThreeDimensionalMatrix {
		List<TwoDimensionalMatrix> allBoards = new List<TwoDimensionalMatrix>();
		int boardSize;
		public ThreeDimensionalMatrix(int numberOfBoards, int boardSize) {
			this.boardSize = boardSize;
			for (int i = 0; i < numberOfBoards; i++) {
				allBoards.Add(new TwoDimensionalMatrix(boardSize));
			}
		}
		public void IncrementPixel(int x, int y, int z){
			allBoards[z].IncrementAt(x, y);
		}
		public void SaveToDisk() {
			for(int i=0; i < allBoards.Count();i++){
				TwoDimensionalMatrix mat = allBoards[i];
				mat.ToBitmap().Save("test" + i.ToString() + ".bmp");
			}
		}
	}

	class TwoDimensionalMatrix {
		public Bitmap ToBitmap() {
			return board.ConvertDoubleArrayToBitmap(Color.White);
		}
		List<List<int>> board = new List<List<int>>();
		int xMid, yMid;
		public TwoDimensionalMatrix(int sideLength) {
			xMid = sideLength / 2;
			yMid = sideLength / 2;
			for (int i = 0; i < sideLength; i++) {
				board.Add(new List<int>());
				for (int j = 0; j < sideLength; j++) {
					board[i].Add(0);
				}
			}
		}
		public void IncrementAt(int x, int y) {
			try {
				board[x + xMid][y + yMid]++;
			} catch { }
		}
	}
}
