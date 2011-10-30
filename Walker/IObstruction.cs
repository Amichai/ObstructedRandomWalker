using System;
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
			//TODO: solve the quadratic formula and get two solutions. Use the incoming angle to determine the correct solutions.
			//Move to the point of collision and reflect from there.
			double root1 = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
			double root2 = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
			var endPt1 = new Vector(startPt.GetX() - root1 * Math.Cos(angle), startPt.GetY() - root1 * Math.Sin(angle));
			var endPt2 = new Vector(startPt.GetX() - root2 * Math.Cos(angle), startPt.GetY() - root2 * Math.Sin(angle));
			if (new Common.LineSegment(startPt, endPt1).Slope() == path.Slope()) {
				return endPt1;
			}
			if (new Common.LineSegment(startPt, endPt1).Slope() == path.Slope()) {
				return endPt2;
			}
			throw new Exception("No collision pt detected");
		}

		public ReflectedLine TestForCollision(Common.LineSegment path) {
			//Built in Ellipse Geometry collision test:
			//var intersect = Geometry.FillContainsWithDetail(new System.Windows.Media.RectangleGeometry(path.AsSystemRect()));
			//if(containsPoint(path.EndingPos) != (intersect != System.Windows.Media.IntersectionDetail.Empty))
			//    throw new Exception();
			//if (intersect != System.Windows.Media.IntersectionDetail.Empty) {

			//My own collision test method found here: http://www.spaceroots.org/documents/distance/node6.html
			//My function may not be accurate in some edge cases. I don't know why.
			if (containsPoint(path.EndingPos)) {
				//GetCollisionPoint(path);
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
