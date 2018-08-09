using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(1)]
	public class Toggle : ISymbol, ISwitch {

		public Toggle() {
		}

		public Toggle(int switchId) {
			this.Id = switchId;
		}

		public int Id { get; }

		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId)
			=> new IBlock[] {
				new Regular(0, 0, BlockType.Solid),
				new Regular(0, 1, BlockType.Solid),
				new Regular(0, 2, BlockType.Solid),
				new Regular(0, 3, BlockType.Solid),
				new Portal(1, 0, BlockType.Portal, 0, curPortalId, curPortalId),
				new Regular(1, 1, BlockType.Down),
				new Switch(1, 2, BlockType.Switch, this.Id),
				new Portal(1, 3, BlockType.Portal, 0, 0, leavingId),
				new Regular(2, 0, BlockType.Solid),
				new Regular(2, 1, BlockType.Solid),
				new Regular(2, 2, BlockType.Solid),
				new Regular(2, 3, BlockType.Solid),
			};

		public ISymbol GetFrom(string[] args) => new Toggle(int.Parse(args[1]));

		public bool Matches(string[] args) => args[0] == "toggle" && int.TryParse(args[1], out var __);

		public int[] ReservedSwitchIds() => new int[] { Id };
	}
}