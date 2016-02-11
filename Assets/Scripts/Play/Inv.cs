//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public class Inv : Attrib{
		public List<Item> items = new List<Item> ();

		public void AddItem(Schema.Item.A a, int count) {
			for (int i = 0; i < count; ++i) {
				Item item = new Item {
					a = a,
				};
				items.Add(item);
			}
		}
	}
}