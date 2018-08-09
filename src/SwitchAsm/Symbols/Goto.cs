using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(1)]
	public class Goto : ISymbol, IPortal {

		public Goto() {
		}

		public Goto(int portalId) {
			this.Id = portalId;
		}

		public int Id { get; }

		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId)
			=> new IBlock[] {
				new Regular(0, 0, BlockType.Solid),
				new Regular(0, 1, BlockType.Solid),
				new Regular(0, 2, BlockType.Solid),
				new Portal(1, 0, BlockType.Portal, 0, curPortalId, curPortalId),
				new Regular(1, 1, BlockType.Down),
				new Portal(1, 2, BlockType.Portal, 0, 0, this.Id),
				new Regular(2, 0, BlockType.Solid),
				new Regular(2, 1, BlockType.Solid),
				new Regular(2, 2, BlockType.Solid),
			};

		public ISymbol GetFrom(string[] args) => new Goto(int.Parse(args[1]));

		public bool Matches(string[] args) => args[0] == "goto" && int.TryParse(args[1], out var __);

		public int[] ReservedPortalIds() => new int[] { Id };
	}
}