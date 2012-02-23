using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeDWalker;
using Common;

namespace PictureWalker {
	class Program {
		static void Main(string[] args) {
			var filepath = @"C:\Users\Amichai\Documents\Visual Studio 2010\Projects\RandomWalker\PictureWalker\bin\Debug\" + "EM-001.jpg";
			var image = new UploadedImage(filepath, new System.Drawing.Rectangle(500, 200, 3500, 3500));
			var heatmap = new Heatmap(ns:2, nt:5, magnification:3, sideLength:20);
			var walker = new Walker(stepSize:.5, startingPt:new Point(image.Width/2, image.Height /2, 0));
			var pictureWalker = new PictureWalker(heatmap, walker, image);
			pictureWalker.Walk(NumberOfSteps:100000);
			pictureWalker.PrintProjections();
			heatmap.Print();
		}
	}
}
