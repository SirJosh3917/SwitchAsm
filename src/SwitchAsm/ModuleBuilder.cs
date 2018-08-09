using System;
using System.Collections.Generic;

namespace SwitchAsm {
	public static class ModuleBuilder {

		public static IEnumerable<Module> CreateFrom(IEnumerable<ISymbol> symbols) {
			var usedPortals = (List<int>)GetUsedPortalIds(symbols);

			var mods = new List<Module>();

			Module workOn = null;

			foreach (var i in symbols) {
				var past = -1;
				if (workOn != null)
					past = workOn.GetId();

				var id = GetUnique(usedPortals);
				usedPortals.Add(id);

				workOn = new Module(id);

				workOn.SetEnd(id);

				workOn.SetBlocks(i.SafelyGetBlocks(id, past));

				mods.Add(workOn);
			}
			return mods;
		}

		private static bool EnumCont(int check, IEnumerable<int> chk) {
			foreach (var i in chk)
				if (i == check)
					return true;
			return false;
		}

		private static int GetUnique(IEnumerable<int> used) {
			var ret = 1;
			while (EnumCont(ret, used)) { ret++; }
			return ret;
		}

		private static IEnumerable<int> GetUsedPortalIds(IEnumerable<ISymbol> symbols) {
			var ret = new List<int>();
			foreach (var i in symbols)
				if (i is IPortal)
					ret.Add(i.Id);
				else if (i is Check chk)
					ret.Add(chk.PortalTeleportTo);
				else if (i is StartFunction fnc)
					ret.Add(fnc.Id);
			return ret;
		}

		private static IEnumerable<int> GetUsedSwitchIds(IEnumerable<ISymbol> symbols) {
			var ret = new List<int>();
			foreach (var i in symbols)
				if (i is ISwitch)
					ret.Add(i.Id);
			return ret;
		}
	}

	public class Module {

		internal Module(int portalId) {
			this._blocks = new List<IBlock>();
			this._pid = portalId;
		}

		private IEnumerable<IBlock> _blocks { get; set; }

		private int _pid { get; set; }

		private int _tpTo { get; set; }

		public IEnumerable<IBlock> GetBlocks()
			=> this._blocks;

		public int GetId() => this._pid;

		public void SetBlocks(IEnumerable<IBlock> blocks)
			=> this._blocks = blocks;

		public void SetEnd(int portalEnd) {
			this._tpTo = portalEnd;
		}
	}
}