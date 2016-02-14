//utf-8。
using System.Collections.Generic;

namespace Play {
	public class Make {

		public readonly int time1;
		public readonly int time2;
		public readonly int sta;
		public readonly ItemSelect[] tools;
		public readonly ItemSelect[] reagents;
		public readonly ItemCreate[] products;

		public Make(int time1, int time2, int sta,
			ItemSelect[] tools,
			ItemSelect[] reagents, 
			ItemCreate[] products)
		{
			this.time1 = time1;
			this.time2 = time2;
			this.sta = sta;
			this.tools = tools;
			this.reagents = reagents;
			this.products = products;
		}
		public bool Can (Entity src) {
			Inv inv = src.GetAttr<Inv>();
			if (inv == null)
				return false;
			List<Item> use = new List<Item>();
			List<Item> all = new List<Item>();
			for (int i = 0; i < tools.Length; ++i) {
				ItemSelect sel = tools[i];
				if (!inv.SelectItem(use, all, sel))
					return false;
				all.AddRange(use);
				use.Clear();
			}
			for (int i = 0; i < reagents.Length; ++i) {
				ItemSelect sel = reagents[i];
				if (!inv.SelectItem(use, all, sel))
					return false;
				all.AddRange(use);
				use.Clear();
			}
			return true;
		}
		//TODO
		public bool Do (Entity src) {
			Ctx ctx = new Ctx {
				src = src,
				dst = null,
			};
			Inv inv = src.GetAttr<Inv>();
			List<Item> use = new List<Item>();
			List<Item> all = new List<Item>();
			for (int i = 0; i < tools.Length; ++i) {
				ItemSelect sel = tools[i];
				if (!inv.SelectItem(use, all, sel))
					return false;
				all.AddRange(use);
				ctx.items.Add(use);
				use = new List<Item>();
			}
			for (int i = 0; i < reagents.Length; ++i) {
				ItemSelect sel = reagents[i];
				if (!inv.SelectItem(use, all, sel))
					return false;
				all.AddRange(use);
				ctx.items.Add(use);
				use = new List<Item>();
			}
			inv.DelItem(all);
			for (int i = 0; i < products.Length; ++i) {
				ItemCreate cre = products[i];
				List<Item> pro = cre.Create(ctx);
				ctx.items.Add(pro);
				inv.AddItem(pro);
			}

			return true;
		}
	}
}
