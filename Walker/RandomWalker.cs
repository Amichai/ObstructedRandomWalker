using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using System.Diagnostics;

namespace Walker {
	class RandomWalker {
		Rectangle boardBounds;
		Random random = new Random();
		IEnumerable<Ellipse> obstructions;
		public RandomWalker(Map map, WalkerForm walkerForm) {
			boardBounds = new Rectangle(0, 0, map.Size.Width, map.Size.Height);
			obstructions = map.Obstructions;
		}
		/// <summary>Returns a new angle in radians</summary>
		double newDirectionGenerator() {
			return ((double)random.Next(360) + random.NextDouble()) * (Math.PI / 180d);
		}
		Vector randomLocationGenerator() {
			int xPos = random.Next(boardBounds.Width);
			int yPos = random.Next(boardBounds.Height);
			return new Vector(xPos, yPos);
		}
		public Vector CurrentPosition;
		public static int stepCounter;
		private int numberOfSteps;

		internal IEnumerable<StatusReport> InitiateRandomWalk(double stepSize, int numberOfSteps, Vector startingPosition = null) {
			this.numberOfSteps = numberOfSteps;
			startingPosition = CurrentPosition;
			if (startingPosition == null) {
				CurrentPosition = new Vector(boardBounds.Width / 2, boardBounds.Height / 2);
			}
			Vector endingPosition = null;
			for (stepCounter = 0; stepCounter < numberOfSteps; stepCounter++) {
				LineSegment newPath = null;
				while (newPath == null){
					double angleToWalk = newDirectionGenerator();
					double x2 = CurrentPosition.GetX() + stepSize * Math.Cos(angleToWalk);
					double y2 = CurrentPosition.GetY() + stepSize * Math.Sin(angleToWalk);
					endingPosition = new Vector(x2, y2);
					newPath = new LineSegment(CurrentPosition, endingPosition);
					if (testForCollision(newPath) != null)
						newPath = null;
				}

				pathWalker(newPath);

				int progressValue = (int)Math.Floor(((double)stepCounter / (double)numberOfSteps) * 100);
				yield return new StatusReport(progressValue, newPath);

				//newPath = testForCollision(newPath);
				//while (newPath != null)	{
				//    pathWalker(newPath);
				//    yield return new StatusReport(progressValue, newPath, "collision");
				//    newPath = testForCollision(newPath);
				//} 
			}
		}

		int horizBoardIdx = 0,
			vertBoardIdx = 0;

		private List<Rectangle> getRectangleForCentersToCheck(LineSegment path) {
			var Rectangles = new List<Rectangle>();
			int width1 = Map.HorizAxis * 2;
			int height1 = Map.VertAxis * 2;
			int leftEdge = (int)path.EndingPos.GetX() - Map.HorizAxis;
			int topEdge = (Map.Height - (int)path.EndingPos.GetY()) - Map.VertAxis ;
			int width2 = int.MinValue, height2 = int.MinValue;
			int wrappedLeftEdge = int.MaxValue, wrappedTopEdge = int.MaxValue, 
				maxWidth = int.MaxValue, maxHeight = int.MaxValue;
			if (leftEdge < 0) {
				wrappedLeftEdge = leftEdge + Map.Width;
				width1 = Math.Abs(leftEdge);
				width2 = Map.HorizAxis * 2 - width1;
				maxWidth = Math.Max(width1, width2);
			} else maxWidth = width1;
			if (topEdge < 0) {
				wrappedTopEdge = topEdge + Map.Height;
				height1 = Math.Abs(topEdge);
				height2 = Map.VertAxis * 2 - height1;
				maxHeight = Math.Max(height1, height2);
			} else maxHeight = height1;
			//TODO: Optimize this code to find the correct rectangular areas
			if (width2 == int.MinValue && height2 == int.MinValue) {
				Rectangles.Add(new Rectangle(leftEdge, topEdge, width1, height1));
				return Rectangles;
			}
			if (width2 != int.MinValue && height2 != int.MinValue) {
				Rectangles.Add(new Rectangle(0, 0, maxWidth, maxHeight));
				Rectangles.Add(new Rectangle(0, topEdge, maxWidth, maxHeight));
				Rectangles.Add(new Rectangle(leftEdge, 0, maxWidth, maxHeight));
				Rectangles.Add(new Rectangle(leftEdge, topEdge, maxWidth, maxHeight));
				return Rectangles;
			}
			if (width2 != int.MinValue) {
				Rectangles.Add(new Rectangle(0, topEdge, maxWidth, maxHeight));
				Rectangles.Add(new Rectangle(wrappedLeftEdge, topEdge, maxWidth, maxHeight));
				return Rectangles;
			}
			if (height2 != int.MinValue) {
				Rectangles.Add(new Rectangle(leftEdge, 0, maxWidth, maxHeight));
				Rectangles.Add(new Rectangle(leftEdge, wrappedTopEdge, maxWidth, maxHeight));
				return Rectangles;
			}
			throw new Exception();
		}

		private LineSegment testForCollision(LineSegment path) {
			List<Rectangle> rectOfCentersToCheck = getRectangleForCentersToCheck(path.reflectOverHorizontalMidLine(Map.Height));
			foreach (Ellipse obst in obstructions.Where(i => rectOfCentersToCheck.ContainsPoint(i.CenterPoint))) {
				return obst.TestForCollision2(path);
			}
			return null;
		}

		List<LineSegment> fullPath = new List<LineSegment>();

		private Vector checkForPassedWalls(LineSegment path) {
			Vector newEndPoint = null;
			if (path.EndingPos.GetX() > Map.Width) {
				horizBoardIdx++;
				newEndPoint = new Vector(path.EndingPos.GetX() - Map.Width, path.EndingPos.GetY());
			}
			if (path.EndingPos.GetX() < 0) {
				horizBoardIdx--;
				newEndPoint = new Vector(path.EndingPos.GetX() + Map.Width, path.EndingPos.GetY());
			}
			if (path.EndingPos.GetY() > Map.Height) {
				vertBoardIdx++;
				newEndPoint = new Vector(path.EndingPos.GetX(), path.EndingPos.GetY() - Map.Height);
			}
			if (path.EndingPos.GetY() < 0) {
				vertBoardIdx--;
				newEndPoint = new Vector(path.EndingPos.GetX(), path.EndingPos.GetY() + Map.Height);
			}
			return newEndPoint;
		}

		private void pathWalker(LineSegment path) {
			stepCounter++;
			fullPath.Add(path);
			var newEndPt = checkForPassedWalls(path);
			if (newEndPt == null)
				CurrentPosition = path.EndingPos;
			else CurrentPosition = newEndPt;
		}

		internal List<LineSegment> GetPathData() {
			return fullPath;
		}
	}

	public static class RectangleExtensionMethods{
		public static bool ContainsPoint(this List<Rectangle> rects, Vector point) {
			foreach (Rectangle rect in rects) {
				if (point.GetX() >= rect.X && point.GetX() <= rect.Right && point.GetY() >= rect.Top && point.GetY() <= rect.Bottom) {
					return true;
				}
			}
			return false;
		}
	}
}
