using System.Collections.Generic;

namespace SwitchAsm {

	public enum BlockType {
		Up = 116,
		Down = 117,
		Left = 114,
		Right = 115,

		Portal = 242,

		Switch = 113,
		Door = 184,
		Gate = 185,

		Solid = 9,
		None = 0,
	}

	public interface IBlock {
		RelativePoint Position { get; }
		BlockType Id { get; }

		IBlock Clone(RelativePoint newPos);

		object[] GetEEArgs(int x, int y);
	}

	public struct Regular : IBlock {
		public Regular(RelativePoint pos, BlockType id) {
			this.Position = pos;
			this.Id = id;
		}

		public Regular(int x, int y, BlockType id) : this(new RelativePoint(x, y), id) {

		}

		public RelativePoint Position { get; }
		public BlockType Id { get; }

		public IBlock Clone(RelativePoint newPos)
			=> new Regular(this.Position.AddTo(newPos), this.Id);

		public object[] GetEEArgs(int x, int y)
			=> new object[] { 0, this.Position.X + x, this.Position.Y + y, (int)this.Id };
	}

	public struct Switch : IBlock {
		public Switch(RelativePoint pos, BlockType id, int switchId) {
			this.Position = pos;
			this.Id = id;
			this.SwitchId = switchId;
		}

		public Switch(int x, int y, BlockType id, int switchId) : this(new RelativePoint(x, y), id, switchId) {

		}

		public RelativePoint Position { get; }
		public BlockType Id { get; }
		public int SwitchId { get; }

		public IBlock Clone(RelativePoint newPos)
			=> new Switch(this.Position.AddTo(newPos), this.Id, this.SwitchId);

		public object[] GetEEArgs(int x, int y)
			=> new object[] { 0, this.Position.X + x, this.Position.Y + y, (int)this.Id, this.SwitchId };
	}

	public struct Portal : IBlock {
		public Portal(RelativePoint pos, BlockType id, PortalDirection rotation, int curId, int targetId) : this(pos, id, new PortalInfo(curId, targetId, rotation)) {

		}

		public Portal(RelativePoint pos, BlockType id, PortalInfo portalInfo) {
			this.Position = pos;
			this.Id = id;
			this.PortalInfo = portalInfo;
		}

		public Portal(int x, int y, BlockType id, PortalDirection rotation, int curId, int targetId) : this(new RelativePoint(x, y), id, new PortalInfo(curId, targetId, rotation)) {

		}

		public RelativePoint Position { get; }
		public BlockType Id { get; }
		public PortalInfo PortalInfo { get; }

		public IBlock Clone(RelativePoint newPos)
			=> new Portal(this.Position.AddTo(newPos), this.Id, this.PortalInfo);

		public object[] GetEEArgs(int x, int y)
			=> new object[] { 0, this.Position.X + x, this.Position.Y + y, (int)this.Id, (int)this.PortalInfo.Rotation, this.PortalInfo.CurrentId, this.PortalInfo.TargetId };
	}

	public struct PortalInfo {
		public PortalInfo(int curId, int targetId, PortalDirection rotate) {
			this.CurrentId = curId;
			this.TargetId = targetId;
			this.Rotation = rotate;
		}

		public int CurrentId;
		public int TargetId;
		public PortalDirection Rotation;
	}

	public enum PortalDirection {
		Up = 2,
		Down = 0,
		Left = 1,
		Right = 3,
	}

	public struct RelativePoint {

		public RelativePoint(int x, int y) {
			this.X = x;
			this.Y = y;
		}

		public int X { get; }
		public int Y { get; }

		public RelativePoint AddTo(RelativePoint other)
			=> new RelativePoint(other.X + this.X, other.Y + this.Y);
	}
}