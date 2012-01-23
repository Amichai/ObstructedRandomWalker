using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Walker {
	class Point {
		public Point(double x, double y, double z){
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public double X, Y, Z;

		//http://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
		/// <summary>Changes the value of the current position based on the random walk values</summary>
		public Point GetNextPt(double distance, double phi, double theta) {
			double x = distance * Math.Cos(phi) * Math.Sin(theta);
			double y = distance * Math.Sin(phi) * Math.Sin(theta);
			double z = distance * Math.Cos(theta);
			return new Point(this.X+x,this.Y+y,this.Z+z);
		}
	}

	class Obstructions {
		private IEnumerable<int> depthsToCheck(Point point) {
			int lowerDepth = (int)Math.Floor(point.Z / dz);
			int upperDepth = (int)Math.Ceiling(point.Z / dz);
			if (point.Z - lowerDepth * dz > dz || point.Z - lowerDepth*dz < 0) {
				throw new Exception();
			}
			if (point.Z - lowerDepth*dz < radius) {				
				yield return lowerDepth;
			}
			if (upperDepth - point.Z * dz > dz || upperDepth - point.Z * dz < 0) {
				throw new Exception();
			}
			if (upperDepth - point.Z * dz < radius) {
				yield return upperDepth;
			}
		}
		private double getAngleOfCylinder(int zValOfCylinder) {
			var angle = (Math.Ceiling((double)zValOfCylinder / layersBeforeRotation) * dtheta);
			if (angle > Math.PI * 2)
				return (angle - Math.PI * 2);
			else return angle;
		}

		private IEnumerable<double> yInterceptsToCheck(double yInterceptForLineThroughPoint, double quantizationOfYIntercepts) {
			var yInterceptIndexValue = yInterceptForLineThroughPoint / quantizationOfYIntercepts;
			int lowerYInterceptIndex = (int)Math.Floor(yInterceptIndexValue);
			int upperYinterceptIndex = (int)Math.Ceiling(yInterceptIndexValue);
			if ((yInterceptIndexValue - lowerYInterceptIndex) * quantizationOfYIntercepts < radius) {
				yield return lowerYInterceptIndex * quantizationOfYIntercepts;
			}
			if((upperYinterceptIndex - yInterceptIndexValue)*quantizationOfYIntercepts < radius){
				yield return upperYinterceptIndex * quantizationOfYIntercepts;
			}
		}

		public bool TestForCollision(Point point) {
			bool hit = false;
			foreach(var zValIndex in depthsToCheck(point)){
				var angle = getAngleOfCylinder(zValIndex);
				var distanceInZ = Math.Abs(point.Z - (zValIndex * dz));
				var slope = Math.Atan(angle);
				var yInterceptForLineThroughPoint = point.Y - slope * point.X;
				var quantizationOfYIntercepts = this.dxy / Math.Sin(angle);
				foreach (var yIntercept in yInterceptsToCheck(yInterceptForLineThroughPoint, quantizationOfYIntercepts)) {
					var distanceInXY = Math.Abs(point.Y - (slope * point.X + yIntercept)) * Math.Cos(angle);
					if (Math.Sqrt(distanceInXY.Sqrd() + distanceInZ.Sqrd()) < radius) {
						if (hit == true)
							throw new Exception("More than one hit");
						hit = true;
						//Optimization:
						//return true;
					}
				}
			}
			return hit;
		}
	

		double dtheta { get; set; }
		int dxy { get; set; }
		int dz { get; set; }
		double radius { get; set; }
		int layersBeforeRotation { get; set; }

		//rotation is happening in the y direction
		//A single layer consists of at least two rows off set by 1/2 dx

		/// <summary>Makes the fibrous obstruction consisting of an infinite amount of layers of infinitely long 
		/// cylinders packed together. Each layer is rotated an angle dtheta from the layer below</summary>
		/// <param name="dtheta">angle of rotation between two consecutive layers</param>
		/// <param name="dx">horizontal distance between the center points of packed cylinders in a single layer</param>
		/// <param name="dy">vertical distance between the center points of packed cylinders in a single layer</param>
		/// <param name="radius">radius of the cylinder</param>
		public Obstructions(
			double dtheta, int dXY, int dZ, double radius, int layersBeforeRotation) {
				this.dtheta = dtheta;
				this.dxy = dXY;
				this.dz = dZ;
				this.radius = radius;
				this.layersBeforeRotation = layersBeforeRotation;
		}
	}

	class FullPath {
		List<Point> fullPath { get; set; }
		public void Add(Point pt) {
			fullPath.Add(pt);
		}
	}

	class ThreeDWalker {
		/// <summary>width, length and height</summary>
		Obstructions obstructions { get; set; }
		Point currentPosition { get; set; }
		Random rand = new Random();
		public ThreeDWalker(int boardSize) {
			this.obstructions = new Obstructions(Math.PI / 4, 20, 20, 3, 2);
			this.currentPosition = new Point(0, 0, 0);
		}

		public void Walk(int numberOfSteps, double stepSize, Point startingPt = null) {
			if (startingPt == null)
				startingPt = currentPosition;
			else currentPosition = startingPt;
			bool collision = false;
			Point testPosition = null;
			FullPath fullPath = new FullPath();
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
	}
}
