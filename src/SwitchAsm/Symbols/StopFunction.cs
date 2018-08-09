using System;
using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(0)]
	[NoBlocks]
	public class StopFunction : ISymbol {

		public StopFunction() {
			this.Id = int.MaxValue;
		}

		public int Id { get; }

		// note: noblocks attributes doesn't call this
		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId)
			=> new IBlock[] {
				new Regular(0, 0, BlockType.Solid),
				new Regular(0, 1, BlockType.Solid),
				new Regular(0, 2, BlockType.Solid),
				new Portal(1, 0, BlockType.Portal, PortalDirection.Down, curPortalId, leavingId),
				new Regular(1, 1, BlockType.Down), // basically the way of saying "there's an exception in your code
				new Regular(1, 2, BlockType.Solid),
				new Regular(2, 0, BlockType.Solid),
				new Regular(2, 1, BlockType.Solid),
				new Regular(2, 2, BlockType.Solid),
			};

		public ISymbol GetFrom(string[] args) => new StopFunction();

		public bool Matches(string[] args) => args[0] == "}";
	}
}