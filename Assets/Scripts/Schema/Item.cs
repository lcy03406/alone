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
			Armor,
			Branch,
			Apple,
			Cross,
		}
		static public void Init () {
			Add(ID.Armor, new Item(Schema.SpriteID.d_armor, "Armor"));
			Add(ID.Branch, new Item(Schema.SpriteID.b_tree_pine_winter, "Branch")); //TODO
			Add(ID.Apple, new Item(Schema.SpriteID.b_tree_pine_winter, "Apple")); //TODO
			Add(ID.Cross, new Item(Schema.SpriteID.d_gauntlets, "Cross")); //TODO
		}
	}
}
