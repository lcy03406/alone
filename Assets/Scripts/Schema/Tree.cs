//utf-8。
using System;

namespace Schema {
	public sealed class Tree : SchemaBase<Tree.ID, Tree> {
		public readonly Sprite.A sprite;
		public readonly string name;

		private Tree (Schema.SpriteID spid, string name) {
			this.sprite = Sprite.GetA (spid);
			this.name = name;
		}
		public enum ID {
			None,
			Pine,
		}
		static public void Init () {
			Add (ID.Pine, new Tree (Schema.SpriteID.b_tree_pine, "Pine Tree"));
		}
	}
}

