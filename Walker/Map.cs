using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Walker {
	public class Map {
		public Map(IEnumerable<IObstruction> obstructions, System.Drawing.Size size, System.Drawing.Point startingPoint) {
			this.Obstructions = obstructions;
			this.Size = size;
			this.StartingPoint = startingPoint;
		}

		public IEnumerable<IObstruction> Obstructions { get; set; }

		public System.Drawing.Point StartingPoint { get; set; }

		public System.Drawing.Size Size { get; set; }

		internal static Map MapBuilder() {
			List<Vector> centerPoints = new List<Vector>();
			List<IObstruction> obstructions = new List<IObstruction>();
			System.Drawing.Size Size = new System.Drawing.Size(Width, Height);
			System.Drawing.Point startingPoint = new System.Drawing.Point(Size.Width / 2, Size.Height / 2);
			Random random = new Random();
			obstructions.Add(new Walls(new Vector(0, 0), new Vector(Width, Height)));

			for (int i = 0; i < NumberOfObstructions; i++) {
				int xPos = random.Next(Width);
				int yPos = random.Next(Height);
				centerPoints.Add(new Vector(xPos, yPos));
			}
			foreach (Vector p in centerPoints) {
				int angleOfExtension = random.Next(0, 360);
				int axis1 = random.Next(axisMin, axisMax);
				int axis2 = random.Next(axisMin, axisMax);
				obstructions.Add(new Ellipse(p, axis1 / 2, axis2 / 2));
			}
			return new Map(obstructions, Size, startingPoint);
		}
		#region MapBuilder and Obstruction Properties
		public const int Width = 900,
			Height = 700,
			NumberOfObstructions = 1;
		public const int axisMin = 290, axisMax = 360;
		#endregion

	}
}
