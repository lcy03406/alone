//utf-8。

namespace Schema {
	public sealed class Make : SchemaBase<Make.ID, Make> {
		public readonly Play.Make i;

		private Make(Play.Make i) {
			this.i = i;
		}

		public enum ID {
			None,
			Yeah,
		}
		static public void Init () {
			Add(ID.Yeah, new Make(i: new Play.Make(
				time1: 3,
				time2: 0,
				sta: 3, //TODO
				tools: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 1)
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 2)
				},
                products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(Item.ID.Yeah),
					q_base: 0,
					q_from: new int[] {1},
					q_rand: 1,
					cap_from: new int[] {0},
					count: 1)
				}
			)));
		}
	}
}
