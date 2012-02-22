using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ThreeDWalker {
	public class Obstructions2 {
		///determine the layer in y
		///det. the angle of that layer
		///do a coordinate transformation based on that angle
		///retest for collision
		///
		private int width, height;

		public void AddLayer(TwoDObstructions layer, double angleOffset) {
			layer.SetAngle(angleOffset);
			this.layers.Add(layer);
			this.height = layer.Height();
			this.width = layer.Width();
		}

		private List<TwoDObstructions> layers = new List<TwoDObstructions>();

		internal bool TestForCollision(Point testPosition) {
			int layerIdx = (int)( testPosition.Y / this.height) % this.layers.Count();
			if (layerIdx < 0)
				layerIdx += this.layers.Count();
			double angle = double.MaxValue;
			try {
				angle = this.layers[layerIdx].GetAngle();
			} catch {
				throw new Exception();
			}
			var transformedPt = transformCoordinates(angle, testPosition);
			return this.layers[layerIdx].TestForCollision(transformedPt.CorrectForPeriodicBoundaries(this.width, this.height, double.MinValue));
		}

		private Point transformCoordinates(double theta, Point pt) {
			var xPrime = pt.X * Math.Cos(theta) - pt.Z *Math.Sin(theta);
			var zPrime = pt.X * Math.Sin(theta) + pt.Z * Math.Cos(theta);
			return new Point(xPrime, pt.Y, zPrime);
		}
	}

	public class TwoDObstructions {
		internal TwoDObstructions(List<Point> centerPoints, List<double> radii, int width, int height) {
			this.centerPoints = centerPoints;
			this.radii = radii;
			this.width = width;
			this.height = height;
		}
		private List<Point> centerPoints = null;
		private List<double> radii = null;
		private int width, height;
		private double angle;

		internal bool TestForCollision(Point testPosition) {
			for (int i = 0; i < centerPoints.Count(); i++) {
				var x1 = centerPoints[i].X;
				var y1 = centerPoints[i].Y;
				var x2 = testPosition.X;
				var y2 = testPosition.Y;
				if (distance(x1, y1, x2, y2) < radii[i])
					return true;
			}
			return false;
		}

		private double distance(double x1, double y1, double x2, double y2) {
			return Math.Sqrt((x2 - x1).Sqrd() + (y2 - y1).Sqrd());
		}

		internal void Print() {
			List<double> xVals = centerPoints.Select(i => i.X).ToList();
			List<double> yVals = centerPoints.Select(i => i.Y).ToList();
			Utilities.DrawCircles(width, height, yVals, xVals, radii, 30).Save("obstructions.bmp");
		}

		internal double GetAngle() {
			return this.angle;
		}

		internal void SetAngle(double angleOffset) {
			this.angle = angleOffset;
		}

		internal int Width() {
			return this.width;
		}

		internal int Height() {
			return this.height;
		}
	}


}
