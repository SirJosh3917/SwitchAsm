using System.Collections.Generic;

namespace SwitchAsm {

	public interface IPortal {

		int[] ReservedPortalIds();
	}

	public interface ISwitch {

		int[] ReservedSwitchIds();
	}

	public interface ISymbol {
		int Id { get; }

		IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId);

		ISymbol GetFrom(string[] args);

		bool Matches(string[] args);
	}
}