//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public class Item {
		public Schema.Item.A a;
		public int q;
		public Item(Schema.Item.A a, int q) {
			this.a = a;
			this.q = q;
		}
    }
	public class ItemSelect {
		public Schema.Item.A a;
		public int count;
		public ItemSelect(Schema.Item.A a, int count) {
			this.a = a;
			this.count = count;
		}
		public bool Select(Item item) {
			return item.a == a;
		}
	}
	public class ItemCreate {
		public Schema.Item.A a;
		public int q_base;
		public int[] q_from;
		public int q_rand;
		public int[] cap_from;
		public int count;
		public ItemCreate(Schema.Item.A a, int q_base, int[] q_from, int q_rand, int[] cap_from, int count) {
			this.a = a;
			this.q_base = q_base;
			this.q_from = q_from;
			this.q_rand = q_rand;
			this.cap_from = cap_from;
			this.count = count;
		}
		public List<Item> Create(Ctx ctx) {
			List<Item> to = new List<Item>();
			for (int i = 0; i < count; ++i) {
				//TODO
				Item item = new Item(a, q_base);
				to.Add(item);
			}
            return to;
		}
	}
}