using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(2)]
	public class Reset : ISymbol, ISwitch {

		public Reset() {
		}

		public Reset(int switchId, bool state) {
			this.Id = switchId;
			this.State = state;
		}

		public int Id { get; }
		public bool State { get; }

		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId)
			=> new IBlock[] {
				new Regular(0, 0, BlockType.Solid),
				new Regular(0, 1, BlockType.Solid),
				new Regular(0, 2, BlockType.Solid),
				new Regular(0, 3, BlockType.Solid),
				new Regular(0, 4, BlockType.Solid),
				new Portal(1, 0, BlockType.Portal, 0, curPortalId, curPortalId),
				new Regular(1, 1, BlockType.Down),
				new Regular(1, 2, BlockType.Right),
				new Regular(1, 3, BlockType.Left),
				new Portal(1, 4, BlockType.Portal, 0, 0, leavingId),
				new Regular(2, 0, BlockType.Solid),
				new Switch(2, 1, this.State ? BlockType.Gate : BlockType.Door, this.Id),
				new Switch(2, 2, this.State ? BlockType.Gate : BlockType.Door, this.Id),
				new Switch(2, 3, BlockType.Switch, this.Id),
				new Portal(2, 4, BlockType.Portal, 0, 0, leavingId),
				new Regular(3, 0, BlockType.Solid),
				new Regular(3, 1, BlockType.Solid),
				new Regular(3, 2, BlockType.Solid),
				new Regular(3, 3, BlockType.Solid),
				new Regular(3, 4, BlockType.Solid),
			};

		public ISymbol GetFrom(string[] args) => new Reset(int.Parse(args[1]), args[2] == "on");

		public bool Matches(string[] args) => args[0] == "reset" && int.TryParse(args[1], out var __) && (args[2] == "on" || args[2] == "off");

		public int[] ReservedSwitchIds() => new int[] { Id };
	}
}