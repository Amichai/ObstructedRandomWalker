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
				Angle incomingLineAngle = 180 - path.Angle();
				Angle incidentAngle = path.AngleBetweenPoints(centerPoint);
				Debug.Print("Angle of incoming line: " + incomingLineAngle.ToString());
				Debug.Print("Angle to center line: " + incidentAngle.InDegrees().ToString());
				Angle returnAngle = path.Angle() - incidentAngle * 2;
				Debug.Print("Return Angle: " + returnAngle.ToString());
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
			BoundingRectangle = new System.Windows.Rect(bottomLeft.AsWindowsPoint(), topRight.AsWindowsPoint());
			Geometry = new LineGeometry();
		}
		public ReflectedLine TestForCollision(Common.LineSegment path) {
			Angle returnAngle = null;
			if (path.Angle().InRadians() == 0)
				throw new Exception("This should never happen!");
			if (path.EndingPos.GetX() <= BoundingRectangle.Left || path.EndingPos.GetX() >= BoundingRectangle.Right) {
				returnAngle = new Angle(path.YComponent(), -path.XComponent());
			}
			if (path.EndingPos.GetY() >= BoundingRectangle.Bottom|| path.EndingPos.GetY() <= BoundingRectangle.Top) {
				returnAngle = new Angle(-path.YComponent(), path.XComponent());
			}
			return new ReflectedLine(returnAngle);

		}
	}
}
