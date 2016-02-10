using System;

namespace Schema {
	public sealed class Item : SchemaBase<Item.ID, Item> {
		public readonly Sprite.A sprite;
		public readonly string name;
		private Item (Schema.SpriteID spid, string name) {
			this.sprite = Sprite.GetA (spid);
			this.name = name;
		}
		public enum ID {
			None,
			Armor,
		}
		static public void Init () {
			Add (ID.Armor, new Item (Schema.SpriteID.d_armor, "Armor"));
		}
	}
}
