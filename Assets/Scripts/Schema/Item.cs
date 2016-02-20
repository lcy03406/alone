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
			Add(ID.Stone, new Item(SpriteID.b_stone, "Stone"));
			Add(ID.Branch, new Item(SpriteID.b_tree_pine_winter, "Branch"));
			Add(ID.OakNut, new Item(SpriteID.b_tree_oak_winter, "Oak Nut"));
			Add(ID.Meat, new Item(SpriteID.b_stone, "Meat"));
			Add(ID.Bone, new Item(SpriteID.b_stone, "Bone"));
			Add(ID.Cross, new Item(SpriteID.d_gauntlets, "Cross"));
			Add(ID.Knife, new Item(SpriteID.d_pick, "Knife"));
			Add(ID.Axe, new Item(SpriteID.d_pick, "Axe"));
			Add(ID.Armor, new Item(SpriteID.d_armor, "Armor"));
		}
	}
}
