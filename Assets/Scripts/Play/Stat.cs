//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

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

		public bool Has(ID id) {
			return ints.ContainsKey(id);
		}
		public int Get(ID id) {
			Assert.IsTrue(Has(id));
			int value;
			if (ints.TryGetValue(id, out value))
				return value;
			return 0;
		}
	}
}
