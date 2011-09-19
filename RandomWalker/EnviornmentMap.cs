using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;
using Common;

namespace RandomWalker {
	public class EnviornmentMap {
		public System.Drawing.Size Size;
		public EnviornmentMap(List<IObstruction> obstructions, Location bottomRightBound) {
			this.Obstructions = obstructions;
			this.Size = new System.Drawing.Size(bottomRightBound.X, bottomRightBound.Y);
			Obstructions.Add(new FourBorders(new Vector(0, 0), new Vector(Size.Width, Size.Height)));
		}
		public List<IObstruction> Obstructions = new List<IObstruction>();
	}

	public interface IObstruction{
		bool DrawMe {get; set;}
		Angle TestForCollision(LineSegment path);
		Rectangle BoundingRectangle { get; set; }
		Vector CenterPoint { get; set; }
		System.Windows.Media.Geometry Geometry { get; set; }
	}

	public class FourBorders : IObstruction {
		public FourBorders(Vector topLeft, Vector bottomRight) {
			BoundingRectangle = new Rectangle((int)topLeft.GetX(), (int)topLeft.GetY(), (int)bottomRight.GetX(), (int)bottomRight.GetY());
			CenterPoint = null;
			Geometry = null;
			this.DrawMe = false;
		}
		public bool DrawMe {get; set;}
		public Angle TestForCollision(LineSegment path) {
			if (path.Angle().InRadians() == 0)
				throw new Exception("This should never happen!");
			if (path.EndingPos.GetX() <= BoundingRectangle.Left || path.EndingPos.GetX() >= BoundingRectangle.Right){
				return new Angle(path.YComponent(), -path.XComponent());
			}
			if (path.EndingPos.GetY() >= BoundingRectangle.Bottom) {
				return new Angle(-path.YComponent(), path.XComponent());
			}
			if (path.EndingPos.GetY() <= BoundingRectangle.Top) {
				return new Angle(path.YComponent(), path.XComponent()).Negate();
			}
			return null;
		}
		public Rectangle BoundingRectangle { get; set; }
		public Vector CenterPoint { get; set; }
		public System.Windows.Media.Geometry Geometry { get; set; }
	}

	public class Ellipse : IObstruction {
		public Angle TestForCollision(LineSegment path) {
			var line = path.AsLineGeometry();
			var intersect = Geometry.FillContainsWithDetail(line);
			if (intersect != System.Windows.Media.IntersectionDetail.Empty) {
				Angle incidentAngle = path.AngleBetweenPoints(CenterPoint);
				Vector resultantEndPoint = path.StartingPos.RotateAbout(path.EndingPos, new Angle(-incidentAngle.InRadians() *2 ));
				LineSegment resultantVector = new LineSegment(path.EndingPos, resultantEndPoint);

				//Testing code - see if the new line gets away:
				if (Geometry.FillContainsWithDetail(resultantVector.AsLineGeometry()) != System.Windows.Media.IntersectionDetail.Empty) {
					//throw new Exception("You didn't get out");
				}
				//end of testing code
				return resultantVector.Angle();
			}
			return null;
		}

		public Ellipse(Vector centerPt, int radius1, int radius2) {
			this.CenterPoint = centerPt;
			this.Radius1 = radius1;
			this.Radius2 = radius2;
			this.BoundingRectangle = new System.Drawing.Rectangle((int)CenterPoint.GetX() - Radius1,
									(int)CenterPoint.GetY() - Radius2,
									(int)Radius1 * 2,
									(int)Radius2 * 2);
			this.Geometry = new System.Windows.Media.EllipseGeometry(CenterPoint.AsWindowsPoint(), (double)Radius1, (double)Radius2);
			this.DrawMe = true;
		}
		public bool DrawMe { get; set; }
		public Rectangle BoundingRectangle { get; set; }
		public Vector CenterPoint { get; set; }
		public int Radius1, Radius2;
		public System.Windows.Media.Geometry Geometry { get; set; }
	}
}
