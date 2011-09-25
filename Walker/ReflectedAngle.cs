using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Walker {
	public class ReflectedLine {
		public Common.Angle ReturnAngle { get; set; }

		public ReflectedLine(Common.Angle returnAngle) {
			this.ReturnAngle = returnAngle;
		}

		internal Common.LineSegment GetReturnLine(LineSegment incoming) {
			return new LineSegment(incoming.EndingPos, ReturnAngle, incoming.Magnitude());
		}

		public bool TestForEscape(System.Windows.Media.Geometry geometry, LineSegment path) {
			var intersect2 = geometry.FillContainsWithDetail(
					new System.Windows.Media.RectangleGeometry(GetReturnLine(path).AsSystemRect()));
			if (intersect2 != System.Windows.Media.IntersectionDetail.Empty) {
				return false;
			} else return true;
		}
	}
}
