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

	public interface HasName {
		Text name { get; set; }
		Text namepl { get; set; }
	}

	public interface  HasVerb {
		Text verb { get; set; }
		Text verb2p { get; set; }
		Text verb3p { get; set; }
	}
}

