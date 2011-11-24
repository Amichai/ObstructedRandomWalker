using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace Walker {
	public partial class WalkerForm : Form {
		RandomWalker walker;
		Heatmap heatMap = new Heatmap();
		Size mapSize;
		Map map;
		public WalkerForm(Map map) {
			this.map = map;
			mapSize = map.Size;
			InitializeComponent(mapSize);
			this.mapDisplay.map = map;
			walker = new RandomWalker(map, this);
			var startingPt = new Vector(map.Size.Width / 2, map.Size.Height / 2);
			walk(startingPt);	
		}
		private void walk(Vector startingPt, int stepSize = 5, int stepsToTake = 10000) {
			
			foreach (var status in walker.InitiateRandomWalk(startingPt, stepSize, stepsToTake)) {
				heatMap.AddPath((LineSegment)status.LastStep);
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
