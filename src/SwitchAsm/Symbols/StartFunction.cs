using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(1)]
	public class StartFunction : ISymbol, IPortal {

		public StartFunction() {
		}

		public StartFunction(int portalId) {
			this.Id = portalId;
		}

		public int Id { get; }

		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId)
			=> new IBlock[] {
				new Regular(0, 0, BlockType.Solid),
				new Regular(0, 1, BlockType.Solid),
				new Regular(0, 2, BlockType.Solid),
				new Regular(0, 3, BlockType.Solid),
				new Regular(1, 0, BlockType.Solid),
				new Portal(1, 1, BlockType.Portal, 0, this.Id, this.Id),
				new Regular(1, 2, BlockType.Down),
				new Portal(1, 3, BlockType.Portal, 0, 0, leavingId),
				new Regular(2, 0, BlockType.Solid),
				new Regular(2, 1, BlockType.Solid),
				new Regular(2, 2, BlockType.Solid),
				new Regular(2, 3, BlockType.Solid),
			};

		public ISymbol GetFrom(string[] args) => new StartFunction(int.Parse(args[0]));

		public bool Matches(string[] args) => args[1] == "{" && int.TryParse(args[0], out var __);

		public int[] ReservedPortalIds() => new int[] { Id };
	}
}