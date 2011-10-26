using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using System.Diagnostics;

namespace Walker {
	class RandomWalker {
		private Map map;
		private WalkerForm walkerForm;
		private Graphics g;
		Rectangle boardBounds;
		Random random = new Random();
		IEnumerable<IObstruction> obstructions;
		public static System.Drawing.Image PathImage { get; set; }

		public RandomWalker(Map map, WalkerForm walkerForm) {
			PathImage = new Bitmap(map.Size.Width, map.Size.Height);
			g = Graphics.FromImage(PathImage);
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

		internal void InitiateRandomWalk(Vector startingPosition = null, double stepSize = 5, int numberOfSteps = 550000) {
			this.numberOfSteps = numberOfSteps;
			startingPosition = CurrentPosition;
			if (startingPosition == null) {
				CurrentPosition = new Vector(boardBounds.Width / 2, boardBounds.Height / 2);
			}
			Vector endingPosition = null;
			for (stepCounter = 0; stepCounter < numberOfSteps; stepCounter++) {
				double angleToWalk = newDirectionGenerator();
				double x2 = CurrentPosition.GetX() + stepSize * Math.Cos(angleToWalk);
				double y2 = CurrentPosition.GetY() + stepSize * Math.Sin(angleToWalk);
				endingPosition = new Vector(x2, y2);
				LineSegment newPath = new LineSegment(CurrentPosition, endingPosition);
				pathWalker(newPath, Color.Blue);
				testForCollisionAndAdd(newPath);
			}
		}

		int horizBoardIdx = 0,
			vertBoardIdx = 0;

		private List<Rectangle> getRectangleForCentersToCheck(LineSegment path) {
			var Rectangles = new List<Rectangle>();
			int width1 = Map.AxisMax * 2;
			int height1 = Map.AxisMax * 2;
			int leftEdge = (int)path.EndingPos.GetX() - Map.AxisMax ;
			int topEdge = (Map.Height - (int)path.EndingPos.GetY()) - Map.AxisMax ;
			int width2 = int.MinValue, height2 = int.MinValue;
			int wrappedLeftEdge = int.MaxValue, wrappedTopEdge = int.MaxValue, 
				maxWidth = int.MaxValue, maxHeight = int.MaxValue;
			if (leftEdge < 0) {
				wrappedLeftEdge = leftEdge + Map.Width;
				width1 = Math.Abs(leftEdge);
				width2 = Map.AxisMax * 2 - width1;
				maxWidth = Math.Max(width1, width2);
			} else maxWidth = width1;
			if (topEdge < 0) {
				wrappedTopEdge = topEdge + Map.Height;
				height1 = Math.Abs(topEdge);
				height2 = Map.AxisMax * 2 - height1;
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

		private void printRectanglesToBoard(List<Rectangle> rects, LineSegment path) {
			var pen = new Pen(Color.Purple, 2);
			g.DrawRectangles(pen, rects.ToArray());
			Debug.Print(path.ToString());
		}

		private void testForCollisionAndAdd(LineSegment path) {
			ReflectedLine reflectedLine = null;
			List<Rectangle> rectOfCentersToCheck = getRectangleForCentersToCheck(path.reflectOverHorizontalMidLine(Map.Height));
			//if(rectOfCentersToCheck.Count > 1)
			//    printRectanglesToBoard(rectOfCentersToCheck, path.reflectOverHorizontalMidLine(Map.Height));
			foreach (IObstruction obst in obstructions.Where(i => rectOfCentersToCheck.ContainsPoint(i.CenterPoint))) {
				reflectedLine = obst.TestForCollision(path);
				if (reflectedLine != null && reflectedLine.ReturnAngle != null && stepCounter < numberOfSteps) {
					if (!reflectedLine.PassedEscapedFromEllipseTest(obst.Geometry, path)) {
						//This means we didn't get away
						//Print relevant error data!!
						pathWalker(reflectedLine.GetReturnLine(path), Color.Green);
						//stepCounter = numberOfSteps;
					} else {
						pathWalker(reflectedLine.GetReturnLine(path), Color.Red);
						testForCollisionAndAdd(reflectedLine.GetReturnLine(path));
					}
				}
			}
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

		private void pathWalker(LineSegment path, Color color) {
			stepCounter++;
			if (color == null)
				color = Color.Brown;
			printToBoard(path.reflectOverHorizontalMidLine(Map.Height), color);
			fullPath.Add(path);
			var newEndPt = checkForPassedWalls(path);
			if (newEndPt == null)
				CurrentPosition = path.EndingPos;
			else CurrentPosition = newEndPt;
		}

		private void printToBoard(LineSegment path, Color color) {
			Point startingPoint = new Point((int)path.StartingPos.GetX(), boardBounds.Height - (int)path.StartingPos.GetY());
			Point endingPoint = new Point((int)path.EndingPos.GetX(), boardBounds.Height - (int)path.EndingPos.GetY());
			g.DrawLine(new System.Drawing.Pen(color, 1f), startingPoint, endingPoint);
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
