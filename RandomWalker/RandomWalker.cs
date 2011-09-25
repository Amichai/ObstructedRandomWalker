using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
//using System.Windows.Media;
using System.Diagnostics;
using Common;

namespace RandomWalker {
	class RandomWalker {
		FullPathWalked fullPathWalked = new FullPathWalked();
		Rectangle boardBounds;
		Graphics g;
		public static Bitmap testBitmap;
		Random random = new Random();
		List<IObstruction> obstructions;
		public RandomWalker(EnviornmentMap map, Form form) {
			testBitmap = new Bitmap(map.Size.Width, map.Size.Height);
			g = Graphics.FromImage(testBitmap);
			boardBounds = new Rectangle(0,0,map.Size.Width,map.Size.Height);
			obstructions = map.Obstructions;
		}

		/// <summary>Returns a new angle in radians</summary>
		double newDirectionGenerator(){
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
				CurrentPosition = new Vector(boardBounds.Width / 2, boardBounds.Height /2);
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

		private void testForCollisionAndAdd(LineSegment path){
			ReflectionReturnValue reflectedLine = null;
			foreach (IObstruction obst in obstructions) {
				reflectedLine = obst.TestForCollision(path);
				if (reflectedLine != null && reflectedLine.Angle != null && stepCounter < numberOfSteps) {
					reflectedLine.SetLineFromAngleAlone(path);
					reflectedLine.PrintReflectionLine(g);
					pathWalker(reflectedLine.GetLineToReturnOn(), Color.Red);
					//LineSegment reflectedLine = new LineSegment(path.EndingPos, angleToReturnOn, path.Magnitude());
					//pathWalker(reflectedLine, Color.Red);
					
					testForCollisionAndAdd(reflectedLine.GetLineToReturnOn());
				}
			}
		}

		private void pathWalker(LineSegment path, Color color) {
			stepCounter++;
			if (color == null)
				color = Color.Brown;
			printToBoard(path, color);
			fullPathWalked.Add(path);
			CurrentPosition = path.EndingPos;
		}

		private void printToBoard(LineSegment path, Color color){
			Point startingPoint = new Point((int)path.StartingPos.GetX(), (int)path.StartingPos.GetY());
			Point endingPoint = new Point((int)path.EndingPos.GetX(), (int)path.EndingPos.GetY());
			g.DrawLine(new System.Drawing.Pen(color, 1f), startingPoint, endingPoint);
		}
	}

	public class ReflectionReturnValue{
		private LineSegment incomingPath;
		public Angle Angle;
		private LineSegment lineToCenter = null;
		public ReflectionReturnValue(Angle angle)
		{
			this.Angle = angle;
		}
		public ReflectionReturnValue(Angle angle, LineSegment lineToCenter) {
			this.Angle = angle;
			this.lineToCenter = lineToCenter;
		}

		public void SetLineFromAngleAlone(LineSegment incomingPath) {
		}
		LineSegment segmentToReturnOn;
		LineSegment reflectionLine = null;
		public LineSegment GetLineToReturnOn() {
			return segmentToReturnOn;
		}
		public LineSegment GetReflectionLine() {
			if(reflectionLine == null) {
				reflectionLine = new LineSegment(segmentToReturnOn.Magnitude(), lineToCenter.Angle().Adjust(new Angle(90, true)), incomingPath.EndingPos);
			}
			return reflectionLine;
		}

		public void PrintReflectionLine(Graphics g) {
			if (lineToCenter != null) {
				Point startingPoint = new Point((int)GetReflectionLine().StartingPos.GetX(), (int)GetReflectionLine().StartingPos.GetY());
				Point endingPoint = new Point((int)GetReflectionLine().EndingPos.GetX(), (int)GetReflectionLine().EndingPos.GetY());
				g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Green, 2f), startingPoint, endingPoint);
				startingPoint = new Point((int)lineToCenter.StartingPos.GetX(), (int)lineToCenter.StartingPos.GetY());
				endingPoint = new Point((int)lineToCenter.EndingPos.GetX(), (int)lineToCenter.EndingPos.GetY());
				g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Purple, 2f), startingPoint, endingPoint);
				RandomWalker.stepCounter = 1000;
			}
		}
	}


	public class FullPathWalked {
		private List<LineSegment> fullPath = new List<LineSegment>();
		public void Add(LineSegment addMe) {
			fullPath.Add(addMe);
		}
	}
}
