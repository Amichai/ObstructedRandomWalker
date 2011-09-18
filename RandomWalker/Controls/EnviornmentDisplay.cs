using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace RandomWalker.Controls {
	class EnvironmentDisplay : Control {
		public EnvironmentDisplay() {
			SetStyle(ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.Opaque
				   | ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.ResizeRedraw,
					 true);

			ResizeRedraw = true;
		}

		EnviornmentMap map;
		///<summary>Gets or sets the map displayed by the control.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual EnviornmentMap Map {
			get { return map; }
			set { map = value; RecalcLayout(); Invalidate(); }
		}
		protected override void OnLayout(LayoutEventArgs levent) {
			base.OnLayout(levent);
			RecalcLayout();
		}

		#region Layout
		///<summary>Gets the point on the control to draw the top left corner of the map.</summary>
		protected Point MapLocation { get; private set; }
		///<summary>Gets the map coordinates of the top-left corner of the map's bounding box.</summary>
		public Location TopLeft { get; private set; }
		///<summary>Gets the map coordinates of the bottom-right corner of the map's bounding box.</summary>
		public Location BottomRight { get; private set; }
		///<summary>Gets the width in cells of the map's bounding box.</summary>
		protected int MapWidth { get { return BottomRight.X - TopLeft.X; } }
		///<summary>Gets the height in cells of the map's bounding box.</summary>
		protected int MapHeight { get { return BottomRight.Y - TopLeft.Y; } }

		void RecalcLayout() {
			if (Map == null)
				return;

			TopLeft = new Location(0,0);
			BottomRight = new Location(Map.Size.Width, map.Size.Height);

			//The size of each cell in pixels
			ClientSize = new System.Drawing.Size(MapWidth, MapHeight);

			//Get the location in the control to draw the (centered) map.
			MapLocation = new Point(
				(ClientSize.Width) / 2,
				(ClientSize.Height) / 2
			);
		}
		///<summary>Gets the point at the upper left corner of the given map location (between TopLeft and BottomRight).</summary>
		public Point GetPoint(double x, double y) {
			return new Point(
				(int)(MapLocation.X + (x - TopLeft.X)),
				(int)(MapLocation.Y + (TopLeft.Y - y))		//Our Y axis is upside-down
			);
		}
		#endregion

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			if (Map == null) {
				DrawEmptyMessage(e);
				return;
			}
			e.Graphics.FillRectangle(Brushes.White, ClientRectangle);

			//Draw the list of obstructions
			foreach (IObstruction obstruct in map.Obstructions) {
				using (var brush = new Pen(Color.FromArgb(0, 0, 0))) {
					if (obstruct.DrawMe == true) {
						e.Graphics.DrawEllipse(brush, obstruct.BoundingRectangle);
					}
				}
			}
			base.OnPaint(e);	//Raise the Paint event
			e.Graphics.DrawImage(RandomWalker.testBitmap, new Point(0, 0));
			//Draw a dot over (0, 0)
			var origin = GetPoint(0, 0);
			origin.Offset(-2, -2);
			e.Graphics.FillEllipse(Brushes.Red, new Rectangle(origin, new Size(4, 4)));
		}

		private void DrawEmptyMessage(PaintEventArgs e) {
			using (var brush = new LinearGradientBrush(ClientRectangle, Color.AliceBlue, Color.DodgerBlue, LinearGradientMode.Vertical)) {
				e.Graphics.FillRectangle(brush, ClientRectangle);
			}
			using (var font = new Font("Segoe UI", 18))
			using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }) {
				e.Graphics.DrawString("No map selected", font, Brushes.DarkBlue, ClientRectangle, format);
			}
		}
	}
}
