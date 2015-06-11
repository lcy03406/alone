using System;
using UnityEngine;

public partial class Scheme {
	
	public class Item {
		public enum ID {
			None,
			Armor,
			Belt,

			Size,
		}
		public Sprite sprite;
		public string name;
	}
	
	private Item[] items = new Item[(int)Item.ID.Size];
	
	public Item GetItem (Item.ID id) {
		return items [(int)id];
	}

	public Item SetItem (Item.ID id, SchemeSpriteID spid, string name) {
		Item item = new Item ();
		item.sprite =  GetSprite (spid);
		item.name = name;
		items[(int)id] = item;
		return item;
	}

	public void LoadItems () {
		SetItem (Item.ID.Armor, SchemeSpriteID.d_armor, "Armor");
		SetItem (Item.ID.Belt, SchemeSpriteID.d_belt, "Belt");
	}

}