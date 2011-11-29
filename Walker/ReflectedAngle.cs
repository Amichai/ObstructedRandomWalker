using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Walker {
	//Was used for debug functionality. Currently unused.
	public class ReflectedLine {
		public Common.Angle ReturnAngle { get; set; }

		public ReflectedLine(Common.Angle returnAngle) {
			this.ReturnAngle = returnAngle;
		}

		static double overrideMagnitude = double.MinValue;

		internal Common.LineSegment GetReturnLine(LineSegment incoming) {
			if (overrideMagnitude == double.MinValue)
				return new LineSegment(incoming.EndingPos, ReturnAngle, incoming.Magnitude());
			else
				return new LineSegment(incoming.EndingPos, ReturnAngle, overrideMagnitude); ;
		}

		public bool PassedEscapedFromEllipseTest(System.Windows.Media.Geometry geometry, LineSegment path) {
			if (this == null)
				throw new NullReferenceException();
		
			var intersect2 = geometry.FillContainsWithDetail(
					new System.Windows.Media.RectangleGeometry(GetReturnLine(path).AsSystemRect()));
			if (intersect2 != System.Windows.Media.IntersectionDetail.Empty) {
				return false;
			} else return true;
		}

		internal void Extend(double mag) {
			overrideMagnitude = mag * 1.5;
		}
	}
}
