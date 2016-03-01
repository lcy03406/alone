//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.PartID;

namespace Play.Attrs {
	[Serializable]
	public class Grow : Attrib {
		[Serializable]
		public class Part {
			public Schema.Item.A a;
			public int count;
			public int cap;
			public int q;
			public int grow_time;
			public int grow_span;
			public int grow_count;

			public Part(
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

			public ItemCreate GetItem() {
				return new ItemCreate(a: a,
					q_base: q,
					q_from: null,
					q_rand: 0,
					cap_from: null,
					count: count);
			}
		}

		public Dictionary<ID, Part> items;

		public Grow() {
			items = new Dictionary<ID, Part>();
		}

		public Grow(Grow b) {
			items = new Dictionary<ID, Part>(b.items);
		}

		public override void OnBorn() {
			base.OnBorn();
			int time = ent.layer.world.param.time;
			foreach (Part part in items.Values) {
				part.grow_time += time;
			}
		}

		public Part Get(ID id) {
			Part part = null;
			items.TryGetValue(id, out part);
			return part;
		}

		public void Set(ID id, int count) {
			Part part = items[id];
            if (count < 0)
				count = 0;
			else if (count > part.cap)
				count = part.cap;
			items[id].count = count;
		}

		public void Tick(int time) {
			foreach (Part part in items.Values) {
				if (part.grow_time + part.grow_span <= time) {
					int count = part.count + part.grow_count;
					if (count < 0)
						count = 0;
					else if (count > part.cap)
						count = part.cap;
					part.count = count;
					part.grow_time = time;
				}
			}
        }
	}
}
