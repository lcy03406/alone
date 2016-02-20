//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Stat<ID> : Attrib where ID: struct {
		public Dictionary<ID, int> ints;
		public Dictionary<ID, int> caps;

		public Stat() {
			ints = new Dictionary<ID, int>();
			caps = new Dictionary<ID, int>();
		}

		public Stat(Stat<ID> b) {
			ints = new Dictionary<ID, int>(b.ints);
			caps = new Dictionary<ID, int>(b.caps);
		}

		public int Get(ID id) {
			int value;
			if (ints.TryGetValue(id, out value))
				return value;
			return 0;
		}
		public int Cap(ID id) {
			int value;
			if (caps.TryGetValue(id, out value))
				return value;
			return 0;
		}
		public void Set(ID id, int value) {
			int cap = Cap(id);
			int set = value;
            if (value < 0)
				set = 0;
			else if (value > cap)
				set = cap;
			ints[id] = set;
		}
	}
}

namespace Play.Stats {
	public enum Creature {
		HitPoint,
		Stamina,
		Damage,
	}
	public enum Tree {
		Grouth,
	}
}
