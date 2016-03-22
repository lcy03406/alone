//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Equip : Attrib{
		private Dictionary<Schema.EquipSlotID, Item> slots;
		private Dictionary<Item, int> items;

		public bool CanEquip(Item item, int style = 0) {
			List<Schema.ItemEquip> eqlist = item.a.s.equip;
			if (eqlist == null || eqlist.Count < style || style < 0) {
				return false;
			}
			if (items.ContainsKey(item))
				return false;
			Schema.ItemEquip eq = eqlist[style];
			foreach (Schema.EquipSlotID slot in eq.slots) {
				if (!slots.ContainsKey(slot)) {
					return false;
				}
			}
			return true;
		}

		public void EquipOn(Item item, int style) {
			if (!CanEquip(item, style)) {
				Assert.IsTrue(false, "cannot equip " + item.a.s.name);
			}
			Schema.ItemEquip eq = item.a.s.equip[style];
			foreach (Schema.EquipSlotID slot in eq.slots) {
				slots.Add(slot, item);
			}
			items.Add(item, style);
			Stat st = ent.GetAttr<Stat>();
			if (st != null) {
				st.AddEquip(eq.stats);
			}
		}

		public void EquipOff(Item item) {
			if (!items.ContainsKey(item)) {
				return;
			}
			int style = items[item];
			items.Remove(item);
			List<Schema.ItemEquip> eqlist = item.a.s.equip;
			if (eqlist == null || eqlist.Count < style || style < 0) {
				return;
			}
			Schema.ItemEquip eq = eqlist[style];
			foreach (Schema.EquipSlotID slot in eq.slots) {
				Assert.AreEqual(slots[slot], item);
				slots[slot] = null;
			}
			Stat st = ent.GetAttr<Stat>();
			if (st != null) {
				st.DelEquip(eq.stats);
			}
		}
	}
}
