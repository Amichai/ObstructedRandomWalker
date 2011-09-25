using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Walker {
	public partial class MapDisplay : Control {
		public MapDisplay() {
			SetStyle(ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.Opaque
				   | ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.ResizeRedraw,
					 true);
			ResizeRedraw = true;
		}
		public Map map;
		///<summary>Gets or sets the map displayed by the control.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Map Map {
			get { return map; }
			set { map = value; RecalcLayout(); Invalidate(); }
		}

		protected override void OnLayout(LayoutEventArgs levent) {
			base.OnLayout(levent);
			RecalcLayout();
		}

		void RecalcLayout() {
			if (Map == null)
				return;
		}

		protected override void OnPaint(PaintEventArgs pe) {
			pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if (Map == null) {
				return;
			}
			pe.Graphics.FillRectangle(Brushes.White, ClientRectangle);

			foreach (IObstruction obstruct in map.Obstructions) {
				using (var brush = new Pen(Color.FromArgb(0, 0, 0))) {
					if (obstruct.DrawMe == true) {
						Rectangle boundingRect = new Rectangle((int)obstruct.BoundingRectangle.X, (int)obstruct.BoundingRectangle.Y,
													(int)obstruct.BoundingRectangle.Width, (int)obstruct.BoundingRectangle.Height);
						pe.Graphics.DrawEllipse(brush, boundingRect);
					}
				}
			}

			base.OnPaint(pe);
			if (RandomWalker.PathImage != null) {
				pe.Graphics.DrawImage(RandomWalker.PathImage, new Point(0, 0));
			}
			pe.Graphics.FillEllipse(Brushes.Red, new Rectangle(map.StartingPoint, new Size(4, 4)));
		}
	}
}
