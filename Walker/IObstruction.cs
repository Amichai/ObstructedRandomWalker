﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Media;
using System.Diagnostics;

namespace Walker {
	public class Ellipse {
		public Geometry Geometry { get; set; }
		public Vector CenterPoint { get; set; }
		public Ellipse(Vector centerPt, Tuple<int,int> radii) {
			Geometry = new EllipseGeometry(centerPt.ToWindowsPt(), (double)radii.Item1, (double)radii.Item2);
			DrawMe = true;
			BoundingRectangle = Geometry.Bounds;
			CenterPoint = centerPt;

		}

		public Ellipse(Vector centerPt, int radius1, int radius2) {
			Geometry = new EllipseGeometry(centerPt.ToWindowsPt(), (double)radius1, (double)radius2);
			DrawMe = true;
			BoundingRectangle = Geometry.Bounds;
			CenterPoint = centerPt;
		}

		public bool DrawMe {get; set;}
		public System.Windows.Rect BoundingRectangle { get; set; }

		private ReflectedLine computeReturnAngle(Angle A, Angle B, Angle C, Angle incidentAngle, Common.LineSegment path) {
			Angle returnAngle = A + B + C;
			ReflectedLine refLine = new ReflectedLine(returnAngle);
			Angle newIncidentAngle =  refLine.GetReturnLine(path).Flip().AngleBetweenPoints(CenterPoint);
			if (incidentAngle + .05 > newIncidentAngle && incidentAngle - .05 < newIncidentAngle){
				return new ReflectedLine(returnAngle);
			} else return null;
		}

		private bool containsPoint(Vector pt) {
			var xRadius = Geometry.Bounds.Width / 2;
			var yRadius = Geometry.Bounds.Height / 2;
			double val = Math.Pow(((pt.GetX() - CenterPoint.GetX()) / xRadius), 2) + Math.Pow(((pt.GetY() - CenterPoint.GetY()) / yRadius), 2) - 1;
			if (val > 0)
				return false;
			else return true;
		}

		//Taken from: http://www.spaceroots.org/documents/distance/node7.html
		public Vector GetCollisionPoint(Common.LineSegment path) {
			var xRadius = Geometry.Bounds.Width / 2;
			var yRadius = Geometry.Bounds.Height / 2;
			var oneMinusF = yRadius / xRadius;
			Vector startPt = path.StartingPos;
			double angle = path.Angle().InRadians();
			double r = startPt.GetX() - CenterPoint.GetX(),
				   z = startPt.GetY() - CenterPoint.GetY();
			double a = Math.Pow(oneMinusF, 2) * Math.Pow(Math.Cos(angle), 2) + Math.Pow(Math.Sin(angle), 2);
			double b = Math.Pow(oneMinusF, 2) * r * Math.Cos(angle) + z * Math.Sin(angle);
			double c = Math.Pow(oneMinusF, 2) * (Math.Pow(r, 2) - Math.Pow(xRadius, 2)) + Math.Pow(z, 2);
			b = -2 * b;
			double[] roots = new QuadraticEquation(a, b, c).Roots();
			if (roots == null)
				throw new Exception("no roots");
			var endPt1 = new Vector(startPt.GetX() - roots[0] * Math.Cos(angle), startPt.GetY() - roots[0] * Math.Sin(angle));
			var endPt2 = new Vector(startPt.GetX() - roots[1] * Math.Cos(angle), startPt.GetY() - roots[1] * Math.Sin(angle));
			if (new Common.LineSegment(startPt, endPt1).Slope().WithinRange(path.Slope(), .005)) {
				return endPt1;
			}
			if (new Common.LineSegment(startPt, endPt2).Slope().WithinRange(path.Slope(), .005)) {
				return endPt2;
			}
			throw new Exception("No collision pt detected");
		}

		public Common.LineSegment TestForCollision2(Common.LineSegment path) {
			if (containsPoint(path.EndingPos)) {
				Vector collisionPt = GetCollisionPoint(path);
				Angle incidentAngle = path.AngleBetweenPoints(CenterPoint);
				Angle incidentAngleTimesTwo = incidentAngle * 2;
				Angle threesixtyMinus = (new Angle(360, true) - incidentAngleTimesTwo);

				Common.LineSegment lineToCenter = new Common.LineSegment(path.EndingPos, CenterPoint);
				Angle oneEighty = new Angle(180, true);
				//SEARCH FOR THE RETURN ANGLE:
				var outLine = computeReturnAngle(oneEighty, path.Angle(), threesixtyMinus, incidentAngle, path);
				if (outLine != null)
					return new Common.LineSegment(collisionPt, outLine.GetReturnLine(path).Angle(), path.Magnitude());
				outLine = computeReturnAngle(oneEighty, path.Angle(), -threesixtyMinus, incidentAngle, path);
				if (outLine != null)
					return new Common.LineSegment(collisionPt, outLine.GetReturnLine(path).Angle(), path.Magnitude());
				throw new Exception();
			}
			return null;
		}

		public ReflectedLine TestForCollision(Common.LineSegment path) {
			//My own collision test method found here: http://www.spaceroots.org/documents/distance/node6.html
			if (containsPoint(path.EndingPos)) {
				Vector collisionPt = GetCollisionPoint(path);
				Angle incidentAngle = path.AngleBetweenPoints(CenterPoint);
				Angle incidentAngleTimesTwo = incidentAngle * 2;
				Angle threesixtyMinus = (new Angle(360, true) - incidentAngleTimesTwo);

				Common.LineSegment lineToCenter = new Common.LineSegment(path.EndingPos, CenterPoint);
				Angle oneEighty = new Angle(180, true);
				//SEARCH FOR THE RETURN ANGLE:
				var outLine = computeReturnAngle(oneEighty, path.Angle(), threesixtyMinus, incidentAngle, path);
				if (outLine != null)
					return outLine;
				outLine = computeReturnAngle(oneEighty, path.Angle(), -threesixtyMinus, incidentAngle, path);
				if (outLine != null)
					return outLine;

				outLine.Extend(path.Magnitude());
				outLine = computeReturnAngle(oneEighty, path.Angle(), threesixtyMinus, incidentAngle, path);
				if (outLine != null && outLine.PassedEscapedFromEllipseTest(Geometry, path) == true)
					return outLine;
				outLine = computeReturnAngle(oneEighty, path.Angle(), -threesixtyMinus, incidentAngle, path);
				if (outLine != null && outLine.PassedEscapedFromEllipseTest(Geometry, path) == true)
					return outLine;
				throw new Exception();
			}
			return null;
		}
	}
}
