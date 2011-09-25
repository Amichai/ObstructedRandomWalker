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
		Size mapSize;
		Map map;
		public WalkerForm(Map map) {
			this.map = map;
			mapSize = map.Size;
			InitializeComponent(mapSize);
			this.mapDisplay.map = map;
			walker = new RandomWalker(map, this);
			walker.InitiateRandomWalk(new Vector(map.Size.Width / 2, map.Size.Height / 2));
		
		}

		private void Walk_Click(object sender, EventArgs e) {
			InitializeComponent(mapSize);
			walker.InitiateRandomWalk(walker.CurrentPosition);
		}

		private void Reset_Click(object sender, EventArgs e) {
			walker = new RandomWalker(map, this);
			InitializeComponent(mapSize);
		}
	}
}
