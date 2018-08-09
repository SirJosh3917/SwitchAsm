using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchAsm {
	public static class WorldBuilder {
		public static IEnumerable<IBlock> BuildWorld(this IEnumerable<Module> mods, int maxX, int maxY) {
			var blocks = new List<IBlock>();

			foreach (var i in mods) {

				var blks = i.GetBlocks();

				var lowest = blks.LowestBlock();

				var regX = int.MaxValue;
				var regY = int.MaxValue;

				for (var x = 0; x < maxX - lowest.X; x++)
					for (var y = 0; y < maxY - lowest.Y; y++)
						if (blks.FitsInRegion(blocks, x, y)) {
							regX = x;
							regY = y;

							x = maxX; // break out of the loop
							y = maxY;
						}

				var relPoint = new RelativePoint(regX, regY);

				foreach (var j in blks)
					blocks.Add(j.Clone(relPoint));
			}

			return blocks;
		}

		public static bool FitsInRegion(this IEnumerable<IBlock> blocks, IEnumerable<IBlock> world, int x, int y) {
			var relPoint = new RelativePoint(x, y);
			foreach (var i in blocks)
				if (world.BlockExistsAt(i.Position.AddTo(relPoint)))
					return false;
			return true;
		}

		public static bool BlockExistsAt(this IEnumerable<IBlock> blocks, RelativePoint point) {
			foreach (var i in blocks)
				if (i.Position.X == point.X && i.Position.Y == point.Y) return true;
			return false;
		}

		public static RelativePoint LowestBlock(this IEnumerable<IBlock> blocks) {
			var pos = new RelativePoint();

			foreach (var i in blocks) {
				if (i.Position.X > pos.X)
					pos = new RelativePoint(i.Position.X, pos.Y);
				if (i.Position.Y > pos.Y)
					pos = new RelativePoint(pos.X, i.Position.Y);
			}

			return pos;
		}
	}
}