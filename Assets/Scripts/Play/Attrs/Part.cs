//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.PartID;

namespace Play.Attrs {
	[Serializable]
	public class Part : Attrib {
		[Serializable]
		public class PartItem {
			public Schema.Item.A a;
			public int count;
			public int cap;
			public int q;
			public int grow_time;
			public int grow_span;
			public int grow_count;

			public PartItem(
				Schema.Item.A a,
				int count,
				int cap,
				int q,
				int grow_span,
				int grow_count)
			{
				this.a = a;
				this.count = count;
				this.cap = cap;
				this.q = q;
				this.grow_time = 0;
				this.grow_span = grow_span;
				this.grow_count = grow_count;
			}

			public PartItem(PartItem b) {
				this.a = b.a;
				this.count = b.count;
				this.cap = b.cap;
				this.q = b.q;
				this.grow_time = b.grow_time;
				this.grow_span = b.grow_span;
				this.grow_count = b.grow_count;
			}

			public ItemCreate GetItem() {
				return new ItemCreate(a: a,
					q_base: q,
					q_from: null,
					q_rand: 0,
					cap_from: null,
					count: count);
			}
		}

		public Dictionary<ID, PartItem> items;

		public Part() {
			items = new Dictionary<ID, PartItem>();
		}

		public Part(Part b) {
			items = new Dictionary<ID, PartItem>();
			foreach (KeyValuePair<ID, PartItem> pair in b.items) {
				items.Add(pair.Key, new PartItem(pair.Value));
			}
		}

		public override void OnBorn() {
			base.OnBorn();
			int time = ent.layer.world.param.time;
			foreach (PartItem part in items.Values) {
				part.grow_time += time;
				SetNextTick(part.grow_time);
			}
		}

		public PartItem Get(ID id) {
			PartItem part = null;
			items.TryGetValue(id, out part);
			return part;
		}

		public void Set(ID id, int count) {
			PartItem part = items[id];
            if (count < 0)
				count = 0;
			else if (count > part.cap)
				count = part.cap;
			items[id].count = count;
		}

		public void AddPart(ID id, PartItem part) {
			items.Add(id, part);
			SetNextTick(part.grow_time);
        }

		public sealed override void Tick(int time, List<string> logs) {
			foreach (PartItem part in items.Values) {
				if (part.grow_span > 0 && part.grow_time + part.grow_span <= time) {
					int count = part.count + part.grow_count;
					if (count < 0)
						count = 0;
					else if (count > part.cap)
						count = part.cap;
					part.count = count;
					part.grow_time = time + part.grow_span;
					SetNextTick(part.grow_time);
				}
			}
        }
	}
}
