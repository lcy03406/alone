//utf-8。
using System.Collections.Generic;
using System.Linq;

namespace Play {
	public class Make {

		public readonly int time1;
		public readonly int time2;
		public readonly Effect eff;
		public readonly EffDecStat<Creature.Stat.ID> sta;
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
			this.ef = new EffMulti(eff: new Effect[] {
					new EffDecStat<Creature.Stat.ID> (new Src(), Creature.Stat.ID.Stamina, sta),
					new EffUseItem(new Src(), tools)
					new Play.EffAddItem (
						ent : new Src(),
						item : new CalcPartItem(
							ent : new Play.Dst(),
							part : part
						),
						count : new Play.Const<int>(count)
					)
				}

			this.sta = new EffDecStat<Creature.Stat.ID>(new Src(), Creature.Stat.ID.Stamina, sta);
			this.tools = tools;
			this.reagents = reagents;
			this.products = products;
		}
		public bool Can (Entity src) {
			Ctx ctx = new Ctx {
				src = src,
				dst = null,
			};
			if (!sta.Can(ctx))
				return false;
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
