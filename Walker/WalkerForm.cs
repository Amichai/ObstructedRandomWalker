using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using SLaks.Progression.Display.WinForms;

namespace Walker {
	public partial class WalkerForm : Form {
		RandomWalker walker;
		Heatmap heatMap = new Heatmap();
		Size mapSize;
		Map map;
		private Graphics g;
		public static System.Drawing.Image PathImage { get; set; }
		public WalkerForm(Map map) {
			PathImage = new Bitmap(map.Size.Width, map.Size.Height);
			g = Graphics.FromImage(PathImage);
			this.map = map;
			mapSize = map.Size;
			InitializeComponent(mapSize);
			this.mapDisplay.map = map;
			walker = new RandomWalker(map, this);
			var startingPt = new Vector(map.Size.Width / 2, map.Size.Height / 2);
			walk(startingPt);
		}

		private void drawLine(Pen pen, LineSegment lineSeg) {
			Point startingPoint = new Point((int)((LineSegment)lineSeg).StartingPos.GetX(), map.Size.Height 
									- (int)((LineSegment)lineSeg).StartingPos.GetY());
			Point endingPoint = new Point((int)((LineSegment)lineSeg).EndingPos.GetX(), map.Size.Height 
									- (int)((LineSegment)lineSeg).EndingPos.GetY());
			g.DrawLine(pen, startingPoint, endingPoint);
		}

		private void walk(Vector startingPt, int stepSize = 5, int stepsToTake = 10000) {
			foreach (var status in walker.InitiateRandomWalk(startingPt, stepSize, stepsToTake)) {
				LineSegment step = (LineSegment)status.LastStep;
				if(status.Tags.Contains("collision"))
					drawLine(new Pen(Color.Red, 1f), step);
				else
					drawLine(new Pen(Color.Blue, 1f), step);
				heatMap.AddPath(step);
			}
		}

		private void Print_Click(object sender, EventArgs e) {
			heatMap.Print();
		}

		private void Walk_Click(object sender, EventArgs e) {
			InitializeComponent(mapSize);
			walk(walker.CurrentPosition);
		}

		private void Reset_Click(object sender, EventArgs e) {
			walker = new RandomWalker(map, this);
			InitializeComponent(mapSize);
		}
	}
}
