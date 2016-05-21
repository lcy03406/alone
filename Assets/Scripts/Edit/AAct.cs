//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public enum TargetSelect {
		Self,
		Pos,
		Unit,
	}
	public struct APrepare {
		public TargetSelect target;
		public int distance;
	}
	public interface MenuItem {
		Text menu { get; set; }
	}
	public abstract class AAct : Meta, MenuItem {
		public Text menu { get; set; }
		public Text verb;
		public APrepare prepare;
		public int cat;
		public Animat ani;
	}

	public sealed class AMenu : Meta, MenuItem {
		public Text menu { get; set; }
		public List<MenuItem> list;
	}
}
