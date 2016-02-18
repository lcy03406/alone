//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Part<ID> : Play.Attrib where ID: struct {
		public Dictionary<ID, Schema.Item.A> items;

		public Part() {
			items = new Dictionary<ID, Schema.Item.A>();
		}

		public Part(Part<ID> b) {
			items = new Dictionary<ID, Schema.Item.A>(b.items);
		}

		public Schema.Item.A Get(ID id) {
			return items[id];
		}
		public void Set(ID id, Schema.Item.A value) {
			items[id] = value;
		}
	}
}

namespace Play.Parts {
	public enum Creature {
		Meat,
		Bone,
	}
	public enum Tree {
		Branch,
		Fruit,
	}
}
