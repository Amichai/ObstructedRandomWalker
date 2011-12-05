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
		static string stateToPrint = string.Empty;

		protected override void OnPaint(PaintEventArgs pe) {
			pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if (Map == null) {
				return;
			}
			pe.Graphics.FillRectangle(Brushes.White, ClientRectangle);

			foreach (Ellipse obstruct in map.Obstructions) {
				using (var brush = new Pen(Color.FromArgb(0, 0, 0))) {
					if (obstruct.DrawMe == true) {
						pe.Graphics.DrawEllipse(brush, new Rectangle((int)obstruct.BoundingRectangle.X, (int)obstruct.BoundingRectangle.Y, (int)obstruct.BoundingRectangle.Width, (int)obstruct.BoundingRectangle.Height));
					}
				}
			}

			base.OnPaint(pe);
			if (WalkerForm.PathImage != null) {
				pe.Graphics.DrawImage(WalkerForm.PathImage, new Point(0, 0));
			}
			pe.Graphics.FillEllipse(Brushes.Red, new Rectangle(map.StartingPoint, new Size(4, 4)));
			Font font = new System.Drawing.Font("arial", 10, FontStyle.Regular);
			pe.Graphics.FillRectangle(Brushes.White, 120,20, 210, 50);
			pe.Graphics.DrawString(stateToPrint, font, Brushes.Black, new PointF(125, 25));
		}
		public void SetTextToPrint(string text) {
			stateToPrint = text;
		}
	}
}
