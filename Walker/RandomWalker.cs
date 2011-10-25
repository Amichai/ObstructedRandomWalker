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

		internal void InitiateRandomWalk(Vector startingPosition = null, double stepSize = 5, int numberOfSteps = 100000) {
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

		private Rectangle getRectangleForCentersToCheck(LineSegment path) {
			int leftEdge = (int)path.EndingPos.GetX() - Map.AxisMax;
			if (leftEdge < 0)
				leftEdge += Map.Width;
			int topEdge = (int)path.EndingPos.GetY() - Map.AxisMax;
			if (topEdge < 0)
				topEdge += Map.Height;
			int width = Map.AxisMax * 2;
			if (width + leftEdge > Map.Width)
			{}
			int height = Map.AxisMax * 2;
			if(height + topEdge > Map.Height)
			{}
			return new Rectangle(leftEdge, topEdge, height, width);
			//TODO: This code is buggy/not working and must be fixed!! The bounding rectangle of centers should wrap around the borders!!!!
		}

		private void testForCollisionAndAdd(LineSegment path) {
			ReflectedLine reflectedLine = null;
			Rectangle rectOfCentersToCheck = getRectangleForCentersToCheck(path);
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
			printToBoard(path, color);
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
		public static bool ContainsPoint(this Rectangle rect, Vector point) {
			if (point.GetX() < rect.X || point.GetX() > rect.Right || point.GetY() < rect.Top || point.GetY() > rect.Bottom)
				return false;
			else return true;
		}
	}
}
