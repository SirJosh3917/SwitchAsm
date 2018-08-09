using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SwitchAsm
{
    public static class ReflectionCrud
    {
		public static ICollection<Type> GetInterfaceInheritorsOf<T>() {
			var types = new List<Type>();

			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
				if (type.GetInterfaces().Contains(typeof(T)))
					types.Add(type);

			foreach (var assembly in GetDependentAssemblies(typeof(ISymbol).Assembly))
				foreach (var type in assembly.GetTypes())
					if (type.GetInterfaces().Contains(typeof(ISymbol))) {
						types.Add(type);
					}

			return types;
		}

		public static ICollection<T> CreateInstances<T>(this IEnumerable<Type> types) {
			var instances = new List<T>();

			foreach (var i in types)
				instances.Add((T)Activator.CreateInstance(i));

			return instances;
		}

		public static bool MeetsAllRequirements(this ISymbol symbol, params string[] args) {
			foreach (var i in symbol.GetType().GetCustomAttributes<RequirementAttribute>())
				if (!i.Meets(args))
					return false;
			return true;
		}

		public static IEnumerable<IBlock> SafelyGetBlocks(this ISymbol symbol, int leavingId, int curPortalId) {
			if (symbol.GetType().GetCustomAttributes(typeof(NoBlocksAttribute), true).Length > 0)
				return Array.Empty<IBlock>();
			else return symbol.GetBlocks(leavingId, curPortalId);
		}

		private static IEnumerable<Assembly> GetDependentAssemblies(Assembly analyzedAssembly) {
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => GetNamesOfAssembliesReferencedBy(a)
									.Contains(analyzedAssembly.FullName));
		}

		private static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly) {
			return assembly.GetReferencedAssemblies()
				.Select(assemblyName => assemblyName.FullName);
		}
	}
}
