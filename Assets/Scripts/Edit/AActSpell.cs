//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public sealed class ASpell : AAct {
		public int time;
		public List<APhase> phase;
	}
	public sealed class AShape {
		public TargetSelect center;
		public int min_range;
		public int max_range;
	}
	public sealed class APhase : Meta {
		public int time;
		public AShape shape;
		public AEffect effect;
	}
}
