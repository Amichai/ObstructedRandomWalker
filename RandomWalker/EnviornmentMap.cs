using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Controls;
using Common;

namespace RandomWalker {
	public class EnviornmentMap {
		public System.Drawing.Size Size;
		public EnviornmentMap(List<IObstruction> obstructions, Location bottomRightBound) {
			this.Obstructions = obstructions;
			this.Size = new System.Drawing.Size(bottomRightBound.X, bottomRightBound.Y);
			Obstructions.Add(new FourBorders(new Position(0, 0), new Position(Size.Width, Size.Height)));
		}
		public List<IObstruction> Obstructions = new List<IObstruction>();
	}

	public interface IObstruction{
		bool DrawMe {get; set;}
		Angle TestForCollision(Vector path);
		Rectangle BoundingRectangle { get; set; }
		Position CenterPoint { get; set; }
		Geometry Geometry { get; set; }
	}

	public class FourBorders : IObstruction {
		public FourBorders(Position topLeft, Position bottomRight) {
			BoundingRectangle = new Rectangle((int)topLeft.GetX(), (int)topLeft.GetY(), (int)bottomRight.GetX(), (int)bottomRight.GetY());
			CenterPoint = null;
			Geometry = null;
			this.DrawMe = false;
		}
		public bool DrawMe {get; set;}
		public Angle TestForCollision(Vector path) {
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
		public Position CenterPoint { get; set; }
		public Geometry Geometry { get; set; }
	}

	public class Ellipse : IObstruction {
		public Angle TestForCollision(Vector path) {
			var line = path.AsLineGeometry();
			var intersect = Geometry.FillContainsWithDetail(line);
			if (intersect != IntersectionDetail.Empty) {
				Vector reflected = path.ReflectLine(new Vector(path.EndingPos, CenterPoint));

				Angle angleToReturn = new Angle(new Vector(path.EndingPos, CenterPoint).Angle().InRadians() * 2 - path.Angle().InRadians());

				//Testing code - see if the new line gets away:
				Vector reflectedLine = new Vector(path.EndingPos, angleToReturn, path.Magnitude());
				if (Geometry.FillContainsWithDetail(reflectedLine.AsLineGeometry()) != IntersectionDetail.Empty) {
					throw new Exception("You didn't get out");
				}
				angleToReturn = new Angle(new Vector(path.EndingPos, CenterPoint).Angle().InRadians() * 2 - path.Angle().InRadians()).Negate();
				Vector reflectedLine1 = new Vector(path.EndingPos, angleToReturn, path.Magnitude());
				if (Geometry.FillContainsWithDetail(reflectedLine1.AsLineGeometry()) != IntersectionDetail.Empty) {
					throw new Exception("You didn't get out");
				}
				//end of testing code
				return angleToReturn;
			}
			return null;
		}

		public Ellipse(Position centerPt, int radius1, int radius2) {
			this.CenterPoint = centerPt;
			this.Radius1 = radius1;
			this.Radius2 = radius2;
			this.BoundingRectangle = new System.Drawing.Rectangle((int)CenterPoint.GetX() - Radius1,
									(int)CenterPoint.GetY() - Radius2,
									(int)Radius1 * 2,
									(int)Radius2 * 2);
			this.Geometry = new EllipseGeometry(CenterPoint.AsWindowsPoint(), (double)Radius1, (double)Radius2);
			this.DrawMe = true;
		}
		public bool DrawMe { get; set; }
		public Rectangle BoundingRectangle { get; set; }
		public Position CenterPoint { get; set; }
		public int Radius1, Radius2;
		public Geometry Geometry { get; set; }
	}
}
