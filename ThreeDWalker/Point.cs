using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeDWalker {
	public class Point {
		public Point(double x, double y, double z) {
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public double X, Y, Z;

		public static Point operator *(Point p1, int p2){
			return new Point(p1.X * p2, p1.Y * p2, p1.Z * p2);
		}

		//http://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
		/// <summary>Changes the value of the current position based on the random walk values</summary>
		public Point GetNextPt(double distance, double phi, double theta) {
			Random rand = new Random();
			double cosTheta = rand.NextDouble() * 2 -1;
			double rho = Math.Sqrt(1 - cosTheta * cosTheta) * distance;
			double x = Math.Cos(phi) * rho;
			double y =  Math.Sin(phi) * rho;
			double z = distance * Math.Cos(theta);
			return new Point(this.X + x, this.Y + y, this.Z + z);
		}

		public Point CorrectForPeriodicBoundaries(double width, double height, double depth = double.MinValue){
			if (width != double.MinValue && X > 0) {
				X -= ((int)X / (int)width) * width;
			}
			if (height != double.MinValue && Y > 0) {
				Y -= ((int)Y / (int)height) * height;
			}
			if (depth != double.MinValue && Z > 0) {
				Z -= ((int)Z / (int)depth) * depth;
			}
			if (width != double.MinValue && X < 0) {
				X -= (((int)X / (int)width)-1) * width;
			}
			if (height != double.MinValue && Y < 0) {
				Y -= (((int)Y / (int)height) -1) * height;
			}
			if (depth != double.MinValue && Z < 0) {
				Z -= (((int)Z / (int)depth) -1) * depth;
			}
			if (depth != double.MinValue &&( Z < 0 || Z > depth))
				throw new Exception();
			if (width != double.MinValue &&(X < 0 || X > width))
				throw new Exception();
			if (height != double.MinValue && (Y < 0 || Y > height))
				throw new Exception();
			
			return new Point(X, Y, Z);
		}
	}
}
