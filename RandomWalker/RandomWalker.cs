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
		private static int stepCounter;
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
				pathWalker(newPath);
				testForCollisionAndAdd(newPath);
			}
		}
		private void testForCollisionAndAdd(LineSegment path){
			Angle angleToReturnOn;
			foreach (IObstruction obst in obstructions) {
				angleToReturnOn = obst.TestForCollision(path);
				if (angleToReturnOn != null && stepCounter < numberOfSteps) {
					LineSegment reflectedLine = new LineSegment(path.EndingPos, angleToReturnOn, path.Magnitude());
					pathWalker(reflectedLine);
					testForCollisionAndAdd(reflectedLine);
				}
			}
		}
		private void pathWalker(LineSegment path){
			stepCounter++;
			g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue, 1f), (int)path.StartingPos.GetX(), (int)path.StartingPos.GetY(), (int)path.EndingPos.GetX(), (int)path.EndingPos.GetY());
			fullPathWalked.Add(path);
			CurrentPosition = path.EndingPos;
		}
	}

	public class FullPathWalked {
		private List<LineSegment> fullPath = new List<LineSegment>();
		public void Add(LineSegment addMe) {
			fullPath.Add(addMe);
		}
	}
}
