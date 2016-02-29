using System;

namespace Schema {
	public sealed class Item : SchemaBase<Item.ID, Item> {
		public readonly Sprite.A sprite;
		public readonly string name;
		private Item (SpriteID spid, string name) {
			this.sprite = Sprite.GetA (spid);
			this.name = name;
		}
		public enum ID {
			Stone,
			Branch,
			OakNut,
			Meat,
			Bone,
			Cross,
			Knife,
			Axe,
			Armor,
		}
		static public void Init () {
			Add(ID.Stone, new Item(SpriteID.b_stone, "stone"));
			Add(ID.Branch, new Item(SpriteID.b_tree_pine_winter, "branch"));
			Add(ID.OakNut, new Item(SpriteID.b_tree_oak_winter, "oak nut"));
			Add(ID.Meat, new Item(SpriteID.b_stone, "meat"));
			Add(ID.Bone, new Item(SpriteID.b_stone, "bone"));
			Add(ID.Cross, new Item(SpriteID.d_gauntlets, "cross"));
			Add(ID.Knife, new Item(SpriteID.d_pick, "knife"));
			Add(ID.Axe, new Item(SpriteID.d_pick, "axe"));
			Add(ID.Armor, new Item(SpriteID.d_armor, "armor"));
		}
	}
}
