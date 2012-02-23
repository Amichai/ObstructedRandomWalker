using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;
using ThreeDWalker;
using Point = ThreeDWalker.Point;

namespace PictureWalker {
	class PictureWalker {
		Heatmap heatmap = null;
		Walker walker = null;
		FullPath path = new FullPath();
		UploadedImage image = null;
		public PictureWalker(Heatmap heatmap, Walker walker, UploadedImage image) {
			this.heatmap = heatmap;
			this.walker = walker;
			this.image = image;
		}

		public void Walk(int NumberOfSteps) {
			int intensity = 0;
			foreach (var A in walker.Walk(NumberOfSteps)) {
				path.Add((Point)A.LastStep);
				intensity = getScaledIntensityVal((Point)A.LastStep);
				heatmap.AddStep((Point)A.LastStep, intensity);
			}
		}

		private int getScaledIntensityVal(Point point) {
			point = point.CorrectForPeriodicBoundaries(image.Width, image.Height);
			return image[(int)point.X][(int)point.Y] / 10;
		}

		public void PrintProjections() {
			string filepath = @"C:\Users\Amichai\Documents\Visual Studio 2010\Projects\RandomWalker\PictureWalker\bin\Debug\";
			path.PrintProjections(5, filepath);
		}
	}

	class Walker {
		public Walker(double stepSize, Point startingPt = null) {
			this.stepSize = stepSize;
			if (startingPt == null)
				currentPosition = new Point(0, 0, 0);
			else currentPosition = startingPt;
		}
		double stepSize;
		Point currentPosition = null;
		Random rand = new Random();
		public IEnumerable<StatusReport> Walk(int NumberOfSteps) {
			for (int i = 0; i < NumberOfSteps; i++) {
				//gets a random angle in radians
				double angle1 = ((double)rand.Next(360) + rand.NextDouble()) * (Math.PI / 180d);
				double angle2 = ((double)rand.Next(360) + rand.NextDouble()) * (Math.PI / 180d);
				currentPosition = currentPosition.GetNextPt(stepSize, angle1, angle2);
				yield return new StatusReport((int)((double)i * 100 / NumberOfSteps), currentPosition);
			}
		}
	}
}
