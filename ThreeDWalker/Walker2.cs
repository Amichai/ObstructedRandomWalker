using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common;
using System.Diagnostics;

namespace ThreeDWalker {
	/// <summary>This walker takes 2d obstruction info and avoids in 3d</summary>
	public class Walker2 {
		double width, height;
		public Walker2() {
			TextReader reader = new StreamReader(@"C:\Users\Amichai\Documents\Visual Studio 2010\Projects\RandomWalker\ThreeDWalker\bin\Debug\junk.txt.bs");
			string obstructionData = reader.ReadToEnd();
			var splitData = obstructionData.Split(new char[]{' ', '\n'}, StringSplitOptions.RemoveEmptyEntries);

			this.width = double.Parse(splitData[0]);
			this.height = double.Parse(splitData[1]);
			int numberOfObstructions = int.Parse(splitData[2]);
			List<Point> centerPoints = new List<Point>();
			List<double> radii = new List<double>();
			for (int i = 3; i < splitData.Count(); i++) {
				radii.Add(double.Parse(splitData[i]));
				var pt = new Point(double.Parse(splitData[++i]), 
								double.Parse(splitData[++i]), 0); 
				centerPoints.Add(pt);
			}
			double angleOffset = Math.PI / 2;
			for (int i = 0; i < 4; i++) {
				TwoDObstructions layer = new TwoDObstructions(centerPoints, radii, (int)Math.Ceiling(width), (int)Math.Ceiling(height));
				obstructions.AddLayer(layer, i * angleOffset);
			}
			reader.Close();
		}

		Obstructions2 obstructions = new Obstructions2();

		public FullPath fullPath = new FullPath();
		int numberOfSteps;
		Point currentPosition = null;
		Random rand = new Random();
		//HEATMAP SPECIFICATION HAPPENS HERE
		Heatmap heatmap = new Heatmap(20,3, 20);

		public IEnumerable<StatusReport> Walk(int numberOfSteps, double stepSize, Point startingPt = null) {
			this.numberOfSteps = numberOfSteps;
			if (startingPt == null)
				currentPosition = randomStart();
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
					if (counter++ > 300)
						throw new Exception("Can't find a move");
				} while (collision);
				currentPosition = testPosition;
				heatmap.AddStep(currentPosition);
				fullPath.Add(currentPosition);
				yield return new StatusReport(i / numberOfSteps, currentPosition);
			}
		}

		public void PrintHeatMap() {
			heatmap.Print();
		}

		private Point randomStart() {
			Point pt = null;
			int counter = 0;
			do {
				double xVal = rand.Next(0, (int)width) + rand.NextDouble();
				double yVal = rand.Next(0, (int)height) + rand.NextDouble();
				pt = new Point(xVal, yVal, 0);
				if (counter++ > 200)
					throw new Exception("Can't find a free spot");
			} while (!obstructions.TestForCollision(pt));
			return pt;
		}

		public void Continue() {
			this.Walk(numberOfSteps, 1);
		}
	}
}
