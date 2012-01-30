using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Diagnostics;

namespace ThreeDWalker {
	public class Obstructions {
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

		private bool testForHit(double dxy1, double dxy2, double dz1, double dz2) {
			if (Math.Sqrt(dxy1.Sqrd() + dz1.Sqrd()) < this.radius) {
				return true;
			} else if (Math.Sqrt(dxy2.Sqrd() + dz2.Sqrd()) < this.radius) {
				return true;
			} return false;
		}

		public bool TestForCollision(Point point) {
			bool hit = false;
			var zInUnitsOfDz  =point.Z / this.dz;
			var dz1 = (zInUnitsOfDz - Math.Floor(zInUnitsOfDz)) * this.dz;
			var dz2 = (Math.Ceiling(zInUnitsOfDz) - zInUnitsOfDz)*this.dz;
			if (dz1 < 0 || dz2 < 0 || dz1 > this.dz || dz2 > this.dz)
				throw new Exception();

			var rodAngle1 = Math.Ceiling(Math.Floor(zInUnitsOfDz) / this.layersBeforeRotation) * this.dtheta;
			var rodAngle2 = Math.Ceiling(Math.Ceiling(zInUnitsOfDz) / this.layersBeforeRotation) * this.dtheta;
			//It's possible that these two angles are exactly the same and this duplicity is redundant
			var slope1 = Math.Atan(rodAngle1);
			var slope2 = Math.Atan(rodAngle2);
			var yIntercept1 = point.Y - slope1 * point.X;
			var yIntercept2 = point.Y - slope2 * point.X;
			var yInterceptQuantization1 = this.dxy / Math.Cos(rodAngle1);
			var b = Math.Floor(yIntercept1 / yInterceptQuantization1) * yInterceptQuantization1;
			Func<double, double> f_rod1 = i => slope1 * i + b;
			var dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			Func<double, double> f_rod2 = i => slope2 * i + b;
			var dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Ceiling(yIntercept1 / yInterceptQuantization1) * yInterceptQuantization1;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Floor(yIntercept2 / yInterceptQuantization1) * yInterceptQuantization1;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Ceiling(yIntercept2 / yInterceptQuantization1) * yInterceptQuantization1;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			var yInterceptQuantization2 = this.dxy / Math.Cos(rodAngle2);
			b = Math.Floor(yIntercept1 / yInterceptQuantization1) * yInterceptQuantization2;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Ceiling(yIntercept1 / yInterceptQuantization1) * yInterceptQuantization2;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Floor(yIntercept2 / yInterceptQuantization1) * yInterceptQuantization2;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

			b = Math.Ceiling(yIntercept2 / yInterceptQuantization1) * yInterceptQuantization2;
			f_rod1 = i => slope1 * i + b;
			dxy1 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle1);
			f_rod2 = i => slope2 * i + b;
			dxy2 = Math.Abs(point.Y - f_rod1(point.X)) * Math.Cos(rodAngle2);
			hit = testForHit(dxy1, dxy2, dz1, dz2);

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
