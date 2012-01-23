using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ThreeDWalker {
	class Obstructions {
		private IEnumerable<int> depthsToCheck(Point point) {
			int lowerDepth = (int)Math.Floor(point.Z / dz);
			int upperDepth = (int)Math.Ceiling(point.Z / dz);
			var distanceFromDepth = Math.Abs(point.Z - (lowerDepth * dz));
			if (distanceFromDepth > dz) {
				throw new Exception();
			}
			if (distanceFromDepth < radius) {
				yield return lowerDepth;
			}
			distanceFromDepth = Math.Abs((upperDepth * dz) - point.Z);
			if (distanceFromDepth > dz) {
				throw new Exception();
			}
			if (distanceFromDepth < radius) {
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
			if ((upperYinterceptIndex - yInterceptIndexValue) * quantizationOfYIntercepts < radius) {
				yield return upperYinterceptIndex * quantizationOfYIntercepts;
			}
		}

		public bool TestForCollision(Point point) {
			bool hit = false;
			foreach (var zValIndex in depthsToCheck(point)) {
				var angle = getAngleOfCylinder(zValIndex);
				var distanceInZ = Math.Abs(point.Z - (zValIndex * dz));
				var slope = Math.Atan(angle);
				var yInterceptForLineThroughPoint = point.Y - slope * point.X;
				var quantizationOfYIntercepts = this.dxy / Math.Sin(angle);
				foreach (var yIntercept in yInterceptsToCheck(yInterceptForLineThroughPoint, quantizationOfYIntercepts)) {
					var distanceInXY = Math.Abs(point.Y - (slope * point.X + yIntercept)) * Math.Cos(angle);
					if (Math.Sqrt(distanceInXY.Sqrd() + distanceInZ.Sqrd()) < radius) {
						if (hit == true) { }
							//throw new Exception("More than one hit");
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
}
