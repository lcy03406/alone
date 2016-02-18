//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Inv : Attrib{
		public List<Item> items = new List<Item> ();

		public List<Item> SelectItem(ItemSelect sel, List<Item> all) {
			List<Item> to = new List<Item>();
            int has = 0;
			for (int i = 0; i < items.Count; i++) {
				Item item = items[i];
				if (all.Contains(item))
					continue;
				if (sel.Select(item)) {
					has += 1;
					to.Add(item);
					if (has >= sel.count)
						return to;
				}
			}
			return null;
		}

		public void DelItem(List<Item> list) {
			foreach (Item del in list) {
				bool ok = items.Remove(del);
				Assert.IsTrue(ok);
			}
		}

		public void AddItem(List<Item> list) {
			items.AddRange(list);
		}
	}
}