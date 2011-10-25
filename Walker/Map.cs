using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Drawing;

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
		static Random random = new Random();

		internal static Map MapBuilder() {
			List<IObstruction> obstructions = new List<IObstruction>();
			System.Drawing.Size Size = new System.Drawing.Size(Width, Height);
			System.Drawing.Point startingPoint = new System.Drawing.Point(Size.Width / 2, Size.Height / 2);
			obstructions.Add(new Walls(new Vector(0, 0), new Vector(Width, Height)));
			foreach (Vector p in getCenterPointsAsLattice(new Size(Width, Height))) {
				//obstructions.Add(new Ellipse(p, getRandomAxis()));
				obstructions.Add(new Ellipse(p, AxisMin, AxisMax));
			}
			
			return new Map(obstructions, Size, startingPoint);
		}

		private static Tuple<int, int> getRandomAxis() {
			int axis1 = random.Next(AxisMin, AxisMax);
			int axis2 = random.Next(AxisMin, AxisMax);
			return new Tuple<int, int>(axis1, axis2 );
		}
		#region MapBuilder and Obstruction Properties
		public const int Width = 900,
			Height = 700,
			NumberOfObstructions = 1;
		public const int AxisMin = 10, AxisMax = 10;

		public static IEnumerable<Vector> getCenterPointsAsLattice(Size mapSize) {
			int xIncrement = 30, yIncrement = 30;
			for(int i=0;i < mapSize.Width; i+= xIncrement){
				for (int j = 0; j < mapSize.Height; j+=yIncrement) {
					yield return new Vector(i, j);
				}
			}
		}

		public static IEnumerable<Vector> getRandomCenterPoints() {
			Random random = new Random();
			for (int i = 0; i < NumberOfObstructions; i++) {
				int xPos = random.Next(Width);
				int yPos = random.Next(Height);
				yield return new Vector(xPos, yPos);
			}
		}

		#endregion

	}
}
