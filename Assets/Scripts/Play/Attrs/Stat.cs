//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.StatID;

namespace Play.Attrs {
	[Serializable]
	public class Stat : Attrib {
		public class St {
			public int value;
			public int cap;
			public int buf;
			public SortedList<int, Buf> bufs = new SortedList<int, Buf>();

			public St(int value, int cap) {
				this.value = value;
				this.cap = cap;
			}

			public St(St b) {
				this.value = b.value;
				this.cap = b.cap;
				this.buf = b.buf;
				foreach (KeyValuePair<int, Buf> pair in bufs) {
					int end_time = pair.Key;
					Buf buf = new Buf(pair.Value);
					this.bufs.Add(end_time, buf);
				}
            }
		}

		public class Buf {
			public Schema.BufID id;
			public int value;

			public Buf() {
			}

			public Buf(Buf b) {
				this.id = b.id;
				this.value = b.value;
			}
		}

		public Dictionary<ID, St> ints = new Dictionary<ID, St>();

		public Stat() {
		}

		public Stat(Stat b) {
			foreach (KeyValuePair<ID, St> pair in ints) {
				ID id = pair.Key;
				St st = new St(pair.Value);
				ints.Add(id, st);
			}
		}

		public bool Has(ID id) {
			return ints.ContainsKey(id);
        }

		public int Get(ID id) {
			Assert.IsTrue(Has(id));
			St st;
			if (ints.TryGetValue(id, out st))
				return st.value;
			return 0;
		}

		public int Cap(ID id) {
			Assert.IsTrue(Has(id));
			St st;
			if (ints.TryGetValue(id, out st))
				return st.cap;
			return 0;
		}
		public void Set(ID id, int value) {
			Assert.IsTrue(Has(id));
			St st = ints[id];
			if (st.cap == 0) {
				st.value = value;
				return;
			}
			int cap = st.cap;
			int set = value;
            if (value < 0)
				set = 0;
			else if (value > cap)
				set = cap;
			st.value = set;
		}

		public void AddBuff(Schema.BufID bid, int end_time, Dictionary<ID, int> stats) {
			foreach (KeyValuePair<ID, int> pair in stats) {
				ID id = pair.Key;
				int value = pair.Value;
				if (!Has(id)) {
					continue;
				}
				//
			}
			//
		}
	}
}
