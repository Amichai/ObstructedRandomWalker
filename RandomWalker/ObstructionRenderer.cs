using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common;

namespace RandomWalker {
	class ObstructionRenderer {
		public ObstructionRenderer(MapObstructionProperties properties) {
			List<Position> centerPoints = new List<Position>();
			Random random = new Random();
			for (int i = 0; i < properties.NumberOfObstructions; i++) {
				int xPos = random.Next(properties.Width);
				int yPos = random.Next(properties.Height);
				centerPoints.Add(new Position(xPos, yPos));
			}
			foreach (Position p in centerPoints) {
				int angleOfExtension = random.Next(0, 360);
				int axis1 = random.Next(properties.axisMin, properties.axisMax);
				int axis2 = random.Next(properties.axisMin, properties.axisMax);
				allObstructions.Add(new Ellipse(p, axis1 / 2, axis2 / 2));
			}
			Map = new EnviornmentMap(allObstructions, new Location(properties.Width, properties.Height));
		}
		List<IObstruction> allObstructions = new List<IObstruction>();

		public EnviornmentMap Map { get; set; }
	}

	public class MapObstructionProperties {
		public int Width = 900,
			Height = 700,
			NumberOfObstructions = 1;
		public int axisMin = 290, axisMax = 360;
	}
}
