using System;

namespace SwitchAsm {

	public sealed class MaxArgsAttribute : RequirementAttribute {

		public MaxArgsAttribute(int maximumArgumentAmount) {
			this.MaximumArgumentAmount = maximumArgumentAmount;
		}

		public int MaximumArgumentAmount { get; }

		public override bool Meets(string[] args)
			=> args.Length - 2 < MaximumArgumentAmount;
	}

	public sealed class MinArgsAttribute : RequirementAttribute {

		public MinArgsAttribute(int minimumArgumentAmount) {
			this.MinimumArgumentAmount = minimumArgumentAmount;
		}

		public int MinimumArgumentAmount { get; }

		public override bool Meets(string[] args)
			=> args.Length > MinimumArgumentAmount;
	}

	public sealed class ExactArgsAttribute : RequirementAttribute {
		public ExactArgsAttribute(int exactArgumentAmount) {
			this.ExactArgumentAmount = exactArgumentAmount;
		}

		public int ExactArgumentAmount { get; }

		public override bool Meets(string[] args)
			=> args.Length - 1 == this.ExactArgumentAmount;
	}

	public sealed class NoBlocksAttribute : Attribute {

		public NoBlocksAttribute() {
		}
	}

	public abstract class RequirementAttribute : Attribute {

		public abstract bool Meets(string[] args);
	}
}