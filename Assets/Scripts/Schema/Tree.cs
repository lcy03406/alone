//utf-8。
using System.Collections.Generic;

namespace Schema {
	public sealed class Tree : SchemaBase<Tree.ID, Tree> {
		public enum Part {
			Branch,
			Fruit,
		};
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Dictionary<Part, Item.A> items;
		public readonly Play.Tree.Stat born_stat;
		public readonly Play.Tree.Stat renew_stat;
		public readonly Play.Tree.Stat full_stat;

		Tree(SpriteID sprite,
			string name, 
			Dictionary<Part, Item.A> items,
			Play.Tree.Stat born_stat,
			Play.Tree.Stat renew_stat,
			Play.Tree.Stat full_stat)
		{
			this.sprite = Sprite.GetA(SpriteID.b_tree_pine);
			this.name = "Pine Tree";
			this.items = items;
			this.born_stat = born_stat;
			this.renew_stat = renew_stat;
			this.full_stat = full_stat;
		}

		public enum ID {
			None,
			Pine,
		}
		static public void Init() {
			Add(ID.Pine, new Tree(
				sprite: SpriteID.b_tree_pine,
				name: "Pine Tree",
				items: new Dictionary<Part, Item.A> {
					{ Part.Branch, Item.GetA(Item.ID.Branch) },
					{ Part.Fruit, Item.GetA(Item.ID.Apple) },
				},
				born_stat: new Play.Tree.Stat {
					ints = {
						{ Play.Tree.Stat.ID.Branch, 5 },
						{ Play.Tree.Stat.ID.Fruit, 0 },
					}
				},
				renew_stat: new Play.Tree.Stat {
					ints = {
						{ Play.Tree.Stat.ID.Branch, 2 },
						{ Play.Tree.Stat.ID.Fruit, 10 },
					}
				},
				full_stat: new Play.Tree.Stat {
					ints = {
						{ Play.Tree.Stat.ID.Branch, 5 },
						{ Play.Tree.Stat.ID.Fruit, 10 },
					}
				}
            ));
		}
	}
}

