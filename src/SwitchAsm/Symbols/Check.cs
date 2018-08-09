using System.Collections.Generic;

namespace SwitchAsm {

	[ExactArgs(2)]
	public class Check : ISymbol, ISwitch, IPortal {

		public Check() {
		}

		public Check(int switchId, bool door, int portalTo) {
			this.Id = switchId;
			this.UseDoor = door;
			this.PortalTeleportTo = portalTo;
		}

		public int Id { get; }

		public int PortalTeleportTo { get; }

		public bool UseDoor { get; }

		public IEnumerable<IBlock> GetBlocks(int leavingId, int curPortalId) {
			var blcks = new List<IBlock>();
			for (int i = 0; i < 5; i++)
				blcks.Add(new Regular(0, i, BlockType.Solid));

			for (int i = 0; i < 5; i++)
				if (i != 2)
					blcks.Add(new Regular(2, i, BlockType.Solid));

			blcks.Add(new Portal(1, 0, BlockType.Portal, 0, curPortalId, curPortalId));
			blcks.Add(new Regular(1, 1, BlockType.Down));
			blcks.Add(new Regular(1, 2, BlockType.Right));

			blcks.Add(new Switch(2, 2, UseDoor ? BlockType.Door : BlockType.Gate, this.Id));
			blcks.Add(new Switch(1, 3, UseDoor ? BlockType.Gate : BlockType.Door, this.Id));

			blcks.Add(new Portal(3, 2, BlockType.Portal, PortalDirection.Right, 0, this.PortalTeleportTo));
			blcks.Add(new Portal(1, 4, BlockType.Portal, PortalDirection.Down, 0, leavingId));
			return blcks;
		}

		public ISymbol GetFrom(string[] args) {
			return new Check(int.Parse(args[1]), args[0] == "if", int.Parse(args[2]));
		}

		public bool Matches(string[] args) =>
			// min args is already checked
			(args[0] == "if" || args[0] == "ifnt") && (int.TryParse(args[1], out var __) && int.TryParse(args[2], out __));

		public int[] ReservedPortalIds() => new int[] { PortalTeleportTo };

		public int[] ReservedSwitchIds() => new int[] { Id };
	}
}