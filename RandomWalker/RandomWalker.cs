using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
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
		Position randomLocationGenerator() {
			int xPos = random.Next(boardBounds.Width);
			int yPos = random.Next(boardBounds.Height);
			return new Position(xPos, yPos);
		}
		public Position CurrentPosition;
		private static int stepCounter;
		private int numberOfSteps;
		internal void InitiateRandomWalk(Position startingPosition = null, double stepSize = 20, int numberOfSteps = 1000) {
			this.numberOfSteps = numberOfSteps;
			startingPosition = CurrentPosition;
			if (startingPosition == null) {
				CurrentPosition = new Position(boardBounds.Width / 2, boardBounds.Height /2);
			}
			Position endingPosition = null;
			for (stepCounter = 0; stepCounter < numberOfSteps; stepCounter++) {
				double angleToWalk = newDirectionGenerator();
				double x2 = CurrentPosition.GetX() + stepSize * Math.Cos(angleToWalk);
				double y2 = CurrentPosition.GetY() + stepSize * Math.Sin(angleToWalk);
				endingPosition = new Position(x2, y2);
				Vector newPath = new Vector(CurrentPosition, endingPosition);
				pathWalker(newPath);
				testForCollisionAndAdd(newPath);
			}
		}
		private void testForCollisionAndAdd(Vector path){
			Angle angleToReturnOn;
			foreach (IObstruction obst in obstructions) {
				angleToReturnOn = obst.TestForCollision(path);
				if (angleToReturnOn != null && stepCounter < numberOfSteps) {
					Vector reflectedLine = new Vector(path.EndingPos, angleToReturnOn, path.Magnitude());
					pathWalker(reflectedLine);
					testForCollisionAndAdd(reflectedLine);
				}
			}
		}
		private void pathWalker(Vector path){
			stepCounter++;
			g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue, 1f), (int)path.StartingPos.GetX(), (int)path.StartingPos.GetY(), (int)path.EndingPos.GetX(), (int)path.EndingPos.GetY());
			fullPathWalked.Add(path);
			CurrentPosition = path.EndingPos;
		}
	}

	public class FullPathWalked {
		private List<Vector> fullPath = new List<Vector>();
		public void Add(Vector addMe) {
			fullPath.Add(addMe);
		}
	}
}
