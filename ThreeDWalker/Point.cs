﻿using System;
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

		//http://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
		/// <summary>Changes the value of the current position based on the random walk values</summary>
		public Point GetNextPt(double distance, double phi, double theta) {
			double x = distance * Math.Cos(phi) * Math.Sin(theta);
			double y = distance * Math.Sin(phi) * Math.Sin(theta);
			double z = distance * Math.Cos(theta);
			return new Point(this.X + x, this.Y + y, this.Z + z);
		}

		public Point CorrectForPeriodicBoundaries(double width, double height, double depth){
			if (X < 0)
				X += width;
			if (Y < 0)
				Y += height;
			if (depth!= double.MinValue && Z < 0)
				Z += depth;
			if (X > width)
				X -= width;
			if (Y > height)
				Y -= height;
			if (depth != double.MinValue && Z > depth)
				Z -= depth;
			return new Point(X, Y, Z);
		}
	}
}
