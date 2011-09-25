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

		internal void InitiateRandomWalk(Vector startingPosition = null, double stepSize = 20, int numberOfSteps = 1000) {
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

		private void testForCollisionAndAdd(LineSegment path) {
			ReflectedLine reflectedLine = null;
			foreach (IObstruction obst in obstructions) {
				reflectedLine = obst.TestForCollision(path);
				if (reflectedLine != null && reflectedLine.ReturnAngle != null && stepCounter < numberOfSteps) {
					if (!reflectedLine.TestForEscape(obst.Geometry, path)) {
						//This means we didn't get away
						//Print relevant error data!!
						stepCounter = numberOfSteps;
						pathWalker(reflectedLine.GetReturnLine(path), Color.Green);

						Debug.Print("Point inside obstruction: " + path.EndingPos.ToString());
						Debug.Print("Angle of incoming line: " + path.Angle().InDegrees().ToString());
						Debug.Print("Angle of outgoing line: " + reflectedLine.GetReturnLine(path).Angle().InDegrees().ToString());
						Debug.Print("New endpoint: " + reflectedLine.GetReturnLine(path).EndingPos.ToString());
					} else {
						pathWalker(reflectedLine.GetReturnLine(path), Color.Red);
						testForCollisionAndAdd(reflectedLine.GetReturnLine(path));
					}
				}
			}
		}

		List<LineSegment> fullPath = new List<LineSegment>();

		private void pathWalker(LineSegment path, Color color) {
			stepCounter++;
			if (color == null)
				color = Color.Brown;
			printToBoard(path, color);
			fullPath.Add(path);
			CurrentPosition = path.EndingPos;
		}

		private void printToBoard(LineSegment path, Color color) {
			Point startingPoint = new Point((int)path.StartingPos.GetX(), boardBounds.Height - (int)path.StartingPos.GetY());
			Point endingPoint = new Point((int)path.EndingPos.GetX(), boardBounds.Height - (int)path.EndingPos.GetY());
			g.DrawLine(new System.Drawing.Pen(color, 1f), startingPoint, endingPoint);
		}
	}
}
