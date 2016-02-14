//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public class Inv : Attrib{
		public List<Item> items = new List<Item> ();

		public bool SelectItem(List<Item> to, List<Item> all, ItemSelect sel) {
			int has = 0;
			for (int i = 0; i < items.Count; i++) {
				Item item = items[i];
				if (all.Contains(item))
					continue;
				if (sel.Select(item)) {
					has += 1;
					to.Add(item);
					if (has >= sel.count)
						return true;
				}
			}
			return false;
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