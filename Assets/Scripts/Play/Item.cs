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

		public int GetUsage(Schema.UsageID id) {
			int value = 0;
			a.s.usages.TryGetValue(id, out value);
			return value;
		}
    }

	public class ItemSelect {
		public List<Schema.Item.A> items;
		public Dictionary<Schema.UsageID, int> usages;
		public int count;
		public ItemSelect(List<Schema.Item.A> items, Dictionary<Schema.UsageID, int> usages, int count) {
			this.items = items;
			this.usages = usages;
			this.count = count;
		}
		public ItemSelect(Schema.Item.A item, int count) {
			this.items = new List<Schema.SchemaBase<Schema.ItemID, Schema.Item>.A>() { item };
			this.usages = new Dictionary<Schema.UsageID, int>();
			this.count = count;
		}
		public override string ToString() {
			return Display();
		}
		public string Display() {
			string disp = "";
			if (items.Count == 0) {
				if (usages.Count == 0) {
					disp += "any item";
				} else {
					disp += "any";
				}
			} else {
				disp += items[0].s.name;
				if (items.Count > 1) {
					disp += " etc";
				}
				if (usages.Count > 0) {
					disp += " of";
				}
			}
			if (usages.Count > 0) {
				foreach (KeyValuePair<Schema.UsageID, int> pair in usages) {
					Schema.UsageID id = pair.Key;
					int value = pair.Value;
					if (value > 1) {
						disp += " level " + value;
					}
					disp += " " + id;
				}
			}
			if (count > 0) {
				disp += " x" + count;
			}
			return disp;
		}
		public bool Select(Item item) {
			if (items.Count > 0) {
				if (!items.Contains(item.a)) {
					return false;
				}
			}
			if (usages.Count > 0) {
				foreach (KeyValuePair<Schema.UsageID, int> pair in usages) {
					Schema.UsageID id = pair.Key;
					int value = pair.Value;
					if (item.GetUsage(id) < value) {
						return false;
					}
				}
			}
			return true;
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
		public ItemCreate(Schema.Item.A a, int count) {
			this.a = a;
			this.q_base = 0;
			this.q_from = null;
			this.q_rand = 0;
			this.cap_from = null;
			this.count = count;
		}
		public override string ToString() {
			return Display();
		}
		public string Display() {
			if (count == 0)
				return a.s.name;
			else
				return a.s.name + " x" + count;
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
