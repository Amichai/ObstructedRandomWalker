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
		Vector CenterPoint { get; set; }
	}


	public class Ellipse : IObstruction {
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

		public ReflectedLine TestForCollision(Common.LineSegment path) {
			var intersect = Geometry.FillContainsWithDetail(new System.Windows.Media.RectangleGeometry(path.AsSystemRect()));
			if (intersect != System.Windows.Media.IntersectionDetail.Empty) {
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

	public class Walls : IObstruction {
		public Geometry Geometry { get; set; }
		public bool DrawMe { get; set; }
		public System.Windows.Rect BoundingRectangle { get; set; }
		public Vector CenterPoint { get; set; }

		public Walls(Vector bottomLeft, Vector topRight) {
			DrawMe = false;
			BoundingRectangle = new System.Windows.Rect(bottomLeft.GetX(), bottomLeft.GetY(), topRight.GetX(), topRight.GetY());
			Geometry = new LineGeometry();
			CenterPoint = new Vector((bottomLeft.GetX() + topRight.GetX()) / 2, (bottomLeft.GetY() + topRight.GetY()) / 2);
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
