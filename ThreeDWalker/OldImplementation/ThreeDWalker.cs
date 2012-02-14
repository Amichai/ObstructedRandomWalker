using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ThreeDWalker {
	public class ThreeDWalker {
		/// <summary>width, length and height</summary>
		Obstructions obstructions { get; set; }
		Point currentPosition { get; set; }
		Random rand = new Random();
		public ThreeDWalker() {
			int r = 6;
			this.obstructions = new Obstructions(Math.PI / 2, 3*r, 3*r,r, 4);
			this.currentPosition = new Point(0, 0, r+1);
		}

		public ThreeDWalker(Obstructions obstructions) {
			this.obstructions = obstructions;
			this.currentPosition = new Point(0, 0, 0);
		}
		public FullPath fullPath = new FullPath();
		int numberOfSteps;
		public void Walk(int numberOfSteps, double stepSize, Point startingPt = null) {
			this.numberOfSteps = numberOfSteps;
			if (startingPt == null)
				startingPt = currentPosition;
			else currentPosition = startingPt;
			bool collision = false;
			Point testPosition = null;
			
			for (int i = 0; i < numberOfSteps; i++) {
				int counter = 0;
				do {
					//gets a random angle in radians
					double angle1 = ((double)rand.Next(360) + rand.NextDouble()) * (Math.PI / 180d);
					double angle2 = ((double)rand.Next(360) + rand.NextDouble()) * (Math.PI / 180d);
					testPosition = currentPosition.GetNextPt(stepSize, angle1, angle2);
					collision = obstructions.TestForCollision(testPosition);
					if (counter++ > 30)
						throw new Exception("Can't find a move");
				} while (collision);
				currentPosition = testPosition;
				fullPath.Add(currentPosition);
			}
		}

		public void Continue() {
			this.Walk(numberOfSteps, 1);
		}

		public void Reset() {
			
		}
	}
}
