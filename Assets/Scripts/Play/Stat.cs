//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public class Stat<ID> : Play.Attrib where ID: struct {
		public Dictionary<ID, int> ints;

		public Stat() {
			ints = new Dictionary<ID, int>();
		}

		public Stat(Stat<ID> b) {
			ints = new Dictionary<ID, int>(b.ints);
		}
	}
}
