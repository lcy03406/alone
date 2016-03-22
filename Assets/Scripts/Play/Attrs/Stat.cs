//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.StatID;

namespace Play.Attrs {
	[Serializable]
	public class Stat : Attrib {
		[Serializable]
		public class St {
			public int value;
			public int cap;
			public int buf;
			public SortedList<Schema.BufID, Buf> bufs = new SortedList<Schema.BufID, Buf>();

			public St(int value, int cap) {
				this.value = value;
				this.cap = cap;
			}

			public St(St b) {
				this.value = b.value;
				this.cap = b.cap;
				this.buf = b.buf;
				foreach (KeyValuePair<Schema.BufID, Buf> pair in bufs) {
					Schema.BufID bid = pair.Key;
					Buf buf = new Buf(pair.Value);
					this.bufs.Add(bid, buf);
				}
            }
		}

		[Serializable]
		public class Buf {
			public int value;
			public int end_time;

			public Buf() {
			}

			public Buf(Buf b) {
				this.value = b.value;
				this.end_time = b.end_time;
			}
		}

		public Dictionary<ID, St> ints = new Dictionary<ID, St>();

		public Stat() {
		}

		public Stat(Stat b) {
			foreach (KeyValuePair<ID, St> pair in b.ints) {
				ID id = pair.Key;
				St st = new St(pair.Value);
				ints.Add(id, st);
			}
		}

		public bool Has(ID id) {
			return ints.ContainsKey(id);
        }

		public int Get(ID id) {
			Assert.IsTrue(Has(id), string.Format("no stat {0}", id));
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

		public void AddBuf(Schema.BufID bid, int end_time, Dictionary<ID, int> stats) {
			foreach (KeyValuePair<ID, int> pair in stats) {
				ID id = pair.Key;
				int value = pair.Value;
				St st = null;
				if (!ints.TryGetValue(id, out st))
					continue;
				Buf buf = null;
				if (st.bufs.TryGetValue(bid, out buf)) {
					st.buf += value - buf.value;
					buf.value = value;
					buf.end_time = end_time;
				} else {
					buf = new Buf();
					st.buf += value;
					buf.value = value;
					buf.end_time = end_time;
					st.bufs.Add(bid, buf);
				}
			}
			SetNextTick(end_time);
		}

		public void DelBuf(Schema.BufID bid) {
			foreach (St st in ints.Values) {
				Buf buf = null;
				if (st.bufs.TryGetValue(bid, out buf)) {
					st.buf -= buf.value;
					st.bufs.Remove(bid);
				}
			}
		}

		public void AddEquip(Dictionary<ID, int> stats) {
			foreach (KeyValuePair<ID, int> pair in stats) {
				ID id = pair.Key;
				int value = pair.Value;
				St st = null;
				if (!ints.TryGetValue(id, out st))
					continue;
				st.buf += value;
			}
		}

		public void DelEquip(Dictionary<ID, int> stats) {
			foreach (KeyValuePair<ID, int> pair in stats) {
				ID id = pair.Key;
				int value = pair.Value;
				St st = null;
				if (!ints.TryGetValue(id, out st))
					continue;
				st.buf -= value;
			}
		}

		public sealed override void Tick(int time, List<string> logs) {
			List<Schema.BufID> del = new List<Schema.BufID>();
			foreach (St st in ints.Values) {
				foreach (KeyValuePair<Schema.BufID, Buf> bpair in st.bufs) {
					Schema.BufID bid = bpair.Key;
					Buf buf = bpair.Value;
					if (buf.end_time > 0 && buf.end_time <= time) {
						st.buf -= buf.value;
						del.Add(bid);
					}
				}
				foreach (Schema.BufID bid in del) {
					st.bufs.Remove(bid);
				}
				del.Clear();
			}
		}
	}
}
