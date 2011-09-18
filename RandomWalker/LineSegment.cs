using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Windows.Media;

namespace RandomWalker {
	//private class Vector {
	//    public Vector(Position start, Position end) {
	//        this.StartingPos = start;
	//        this.EndingPos = end;
	//        this.Rise = EndingPos.GetY() - StartingPos.GetY();
	//        this.Run = EndingPos.GetX() - StartingPos.GetX();
	//        this.Angle = new Angle(Rise, Run);
	//        this.Slope = Rise / Run;
	//        this.Length = Math.Sqrt(Math.Pow(Rise, 2) +
	//                                Math.Pow(Run, 2));
	//    }
	//    public Vector(Position start, Angle angle, double length) {
	//        this.StartingPos = start;
	//        this.Length = length;
	//        this.Angle = angle;
	//        this.EndingPos = new Position(StartingPos.GetX() + Length * Math.Cos(Angle.InRadians()),
	//                                    StartingPos.GetY() + Length * Math.Sin(Angle.InRadians()));
	//        this.Rise = EndingPos.GetY() - StartingPos.GetY();
	//        this.Run = EndingPos.GetX() - StartingPos.GetX();
	//        this.Slope = Rise / Run;
	//    }

	//    public LineGeometry AsLineGeometry() {
	//        return new LineGeometry(StartingPos.AsWindowsPoint(), EndingPos.AsWindowsPoint());
	//    }
	//    public double Rise, Run;
	//    public Angle Angle;
	//    public Position StartingPos;
	//    public double Length;
	//    public Position EndingPos;
	//    public double Slope;
	//}
}
