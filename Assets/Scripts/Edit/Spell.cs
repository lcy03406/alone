//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public enum Choose {
		Self,
		Pos,
		Unit,
	}
	public enum AnimatStyle {
		Once,
		Repeat,
		Freeze,
	}
	public struct Animat {
		public int state;
		public AnimatStyle style; 
	}
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
	public sealed class Phase : Data {
		public int time;
		public Choose center;
		public int diameter;
		public int angle;
		public Buff phase_buff;
		public List<Effect> effect;
	}
	public sealed class Effect : Data {
		public EffectFunc func;
		public List<int> param;
	}
	public sealed class EffectFunc : Data {
		public Fun.EffectFunc fun;
	}
}
