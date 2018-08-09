using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwitchAsm {

	public class Parser {
		private Dictionary<string, string> _compReplDefs;

		private List<ISymbol> _stack;

		public Parser() {
			this._compReplDefs = new Dictionary<string, string>();
			this._stack = new List<ISymbol>();
			
			this._definedSymbols = ReflectionCrud.GetInterfaceInheritorsOf<ISymbol>().CreateInstances<ISymbol>();
		}

		private IEnumerable<ISymbol> _definedSymbols { get; }

		public IEnumerable<ISymbol> GetCode() {
			bool inFunc = false;
			foreach (var i in this._stack) {
				if (i is StartFunction && inFunc) throw new Exception("Cannot create function while in function.");
				else if (i is StopFunction && !inFunc) throw new Exception("Cannot stop function when no function is being declared.");

				if (i is StartFunction) inFunc = true;
				else if (i is StopFunction) inFunc = false;

				if (!inFunc && !(i is StartFunction || i is StopFunction)) throw new Exception("You must be in a function to use code.");
			}

			return this._stack.ToArray();
		}

		public void ParseLine(string input) {
			var line = input;
			foreach (var i in _compReplDefs)
				line = line.Replace(i.Key, i.Value);

			line = line.Trim();
			var parse = line.Split(' ', '\t');

			if (line.Length > 0) {
				if (parse[0].StartsWith("#") && parse[0] != "#")
					_compReplDefs[parse[0].Substring(1)] = line.Substring(parse[0].Length).Trim();

				if (parse[0] != "#")
					foreach (var i in this._definedSymbols) {
						
						if (i.MeetsAllRequirements(parse) && i.Matches(parse)) {
							this._stack.Add(i.GetFrom(parse));
							break;
						}
					}
			}
		}
	}
}