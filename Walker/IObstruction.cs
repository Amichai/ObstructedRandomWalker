using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Media;
using System.Diagnostics;

namespace Walker {
	public interface IObstruction {
		bool DrawMe { get; set; }
		ReflectedLine TestForCollision(Common.LineSegment path);
		System.Windows.Rect BoundingRectangle { get; set; }
		Geometry Geometry { get; set; }
	}

	public class Ellipse : IObstruction {
		public Geometry Geometry { get; set; }
		public Ellipse(Vector centerPt, int radius1, int radius2) {
			Geometry = new EllipseGeometry(centerPt.ToWindowsPt(), (double)radius1, (double)radius2);
			DrawMe = true;
			BoundingRectangle = Geometry.Bounds;
			centerPoint = centerPt;
		}
		public bool DrawMe {get; set;}
		public System.Windows.Rect BoundingRectangle { get; set; }
		private Vector centerPoint;

		public ReflectedLine TestForCollision(Common.LineSegment path) {
			var intersect = Geometry.FillContainsWithDetail(new System.Windows.Media.RectangleGeometry(path.AsSystemRect()));
			if (intersect != System.Windows.Media.IntersectionDetail.Empty) {
				Angle incidentAngle = path.AngleBetweenPoints(centerPoint);
				Debug.Print("NEW EVENT LOG ANGLE DATA!:");
				Debug.Print("Angle to center line: " + incidentAngle.InDegrees().ToString());
				Angle incidentAngleTimesTwo = incidentAngle * 2;
				Debug.Print("Angle to center line times two: " + incidentAngleTimesTwo.ToString());
				Angle threesixtyMinus = (new Angle(360, true) - incidentAngleTimesTwo);
				Debug.Print("Three sixty minus angle to center line times two: " + threesixtyMinus.ToString());
				Common.LineSegment lineToCenter = new Common.LineSegment(path.EndingPos, centerPoint);
				Angle returnAngle;
				ReflectedLine refLine;
				Angle oneEighty = new Angle(180, true);
				if (path.Angle().InDegrees() >= 180) {
					returnAngle = oneEighty + path.Angle() + threesixtyMinus;
					refLine = new ReflectedLine(returnAngle);
					if (incidentAngle == refLine.GetReturnLine(path).AngleBetweenPoints(centerPoint)) {
						return new ReflectedLine(returnAngle);
					}
					returnAngle = oneEighty + path.Angle() - threesixtyMinus;
					refLine = new ReflectedLine(returnAngle);
					if (incidentAngle == refLine.GetReturnLine(path).AngleBetweenPoints(centerPoint)) {
						return new ReflectedLine(returnAngle);
					}
				} else {
					returnAngle = oneEighty + path.Angle() + threesixtyMinus;
					refLine = new ReflectedLine(returnAngle);
					if (incidentAngle == refLine.GetReturnLine(path).AngleBetweenPoints(centerPoint)) {
						return new ReflectedLine(returnAngle);
					}
					returnAngle = oneEighty + path.Angle() - threesixtyMinus;
					refLine = new ReflectedLine(returnAngle);
					if (incidentAngle == refLine.GetReturnLine(path).AngleBetweenPoints(centerPoint)) {
						return new ReflectedLine(returnAngle);
					}
				}
				Debug.Print("Return angle: " + returnAngle.ToString());
				throw new Exception("this didn't work");
				return new ReflectedLine(returnAngle);
			}
			return null;
		}

	}

	public class Walls : IObstruction {
		public Geometry Geometry { get; set; }
		public bool DrawMe { get; set; }
		public System.Windows.Rect BoundingRectangle { get; set; }

		public Walls(Vector bottomLeft, Vector topRight) {
			DrawMe = false;
			BoundingRectangle = new System.Windows.Rect(bottomLeft.GetX(), bottomLeft.GetY(), topRight.GetX(), topRight.GetY());
			Geometry = new LineGeometry();
		}
		public ReflectedLine TestForCollision(Common.LineSegment path) {
			Angle returnAngle = null;
			if (path.Angle().InRadians() == 0)
				throw new Exception("This should never happen!");

			if ((path.EndingPos.GetX() <= BoundingRectangle.Left && path.EndingPos.GetY() <= BoundingRectangle.Y)
			|| (path.EndingPos.GetX() >= BoundingRectangle.Right && path.EndingPos.GetY() >= BoundingRectangle.Bottom)
				|| (path.EndingPos.GetX() <= BoundingRectangle.Left && path.EndingPos.GetY() >= BoundingRectangle.Bottom)
				|| (path.EndingPos.GetX() >= BoundingRectangle.Right && path.EndingPos.GetY() <= BoundingRectangle.Y)) {
				returnAngle = new Angle(-path.YComponent(), -path.XComponent());
			} else {
				if (path.EndingPos.GetX() <= BoundingRectangle.Left || path.EndingPos.GetX() >= BoundingRectangle.Right) {
					returnAngle = new Angle(path.YComponent(), -path.XComponent());
				}
				if (path.EndingPos.GetY() <= BoundingRectangle.Y || path.EndingPos.GetY() >= BoundingRectangle.Bottom) {
					returnAngle = new Angle(-path.YComponent(), path.XComponent());
				}
			}
			return new ReflectedLine(returnAngle);

		}
	}
}
