using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace RandomWalker {
	public partial class WalkEnviornment : Form {
		RandomWalker walker;
		EnviornmentMap map;
		public WalkEnviornment(EnviornmentMap map) {
			this.map = map;
			InitializeComponent(map.Size);
			mapDisplay.Map = map;
			walker = new RandomWalker(map, this);
			walker.InitiateRandomWalk(new Position(map.Size.Width / 2, map.Size.Height / 2));
		}

		private void Walk_Click(object sender, EventArgs e) {
			InitializeComponent(map.Size);
			walker.InitiateRandomWalk(walker.CurrentPosition);
		}
	}
}
