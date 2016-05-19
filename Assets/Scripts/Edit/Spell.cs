//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public sealed class Spell : Data {
		public Text name;
		public Text verb2p;
		public Text verb3p;
		public int cat;
		public Choose target;
		public int distance;
		public Animat ani;
		public int time;
		public List<Phase> phase;
	}
	/*
	0 1 3 5
	1 2 4 6
	3 4 5 7
	5 6 7 8
	*/
	public sealed class Shape : Data {
		public Choose center;
		public int min_range;
		public int max_range;
	}
	public sealed class Phase : Data {
		public int time;
		public Shape shape;
		public Buff phase_buff;
		public Effect effect;
	}
	public sealed class Effect : Data {
		public Fun.EffectFunc func;
	}
}
