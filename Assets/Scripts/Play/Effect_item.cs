//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play.Eff {
	public class UseItem : Effect {
		public Calc<Entity> c_ent;
		public Calc<ItemSelect> c_sel;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 2) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			Schema.Item.A a = Schema.Item.GetA((Schema.ItemID)param[0]);
			ItemSelect sel = new ItemSelect(a, param[1]);
			c_sel = new Calcs.Const<ItemSelect>(sel);
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "use " + c_sel.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Inv inv = ent.GetAttr<Inv>();
			if (inv == null)
				return false;
			if (!c_sel.Can(ctx))
				return false;
			ItemSelect sel = c_sel.Get(ctx);
			Ctx.Invx invx = ctx.GetInv(inv);
			List<Item> to = inv.SelectItem(sel, invx.use);
			if (to == null)
				return false;
			invx.use.AddRange(to);
			return true;
		}
		public override void Do(Ctx ctx, List<string> logs) {
		}
	}

	public class DelItem : Effect {
		public Calc<Entity> c_ent;
		public Calc<ItemSelect> c_sel;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 2) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			Schema.Item.A a = Schema.Item.GetA((Schema.ItemID)param[0]);
			ItemSelect sel = new ItemSelect(a, param[1]);
			c_sel = new Calcs.Const<ItemSelect>(sel);
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "consume " + c_sel.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Inv inv = ent.GetAttr<Inv>();
			if (inv == null)
				return false;
			if (!c_sel.Can(ctx))
				return false;
			ItemSelect sel = c_sel.Get(ctx);
			Ctx.Invx invx = ctx.GetInv(inv);
			List<Item> to = inv.SelectItem(sel, invx.use);
			if (to == null)
				return false;
			invx.use.AddRange(to);
			invx.del.AddRange(to);
			return true;
		}
		public override void Do(Ctx ctx, List<string> logs) {
		}
	}

	public class AddItem : Effect {
		public Calc<Entity> c_ent;
		public Calc<ItemCreate> c_cre;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 2) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			Schema.Item.A a = Schema.Item.GetA((Schema.ItemID)param[0]);
			ItemCreate cre = new ItemCreate(a, param[1]);
			c_cre = new Calcs.Const<ItemCreate>(cre);
		}


		public override string Display() {
			return c_ent.Display() + ": "
				+ "make " + c_cre.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Inv inv = ent.GetAttr<Inv>();
			if (inv == null)
				return false;
			if (!c_cre.Can(ctx))
				return false;
			ItemCreate cre = c_cre.Get(ctx);
			List<Item> to = cre.Create(ctx);
			if (to == null)
				return false;
			return true;
		}
		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Inv inv = ent.GetAttr<Inv>();
			ItemCreate cre = c_cre.Get(ctx);
			List<Item> to = cre.Create(ctx);
			inv.AddItem(to);
			if (logs != null) {
				logs.Add(ent.GetName() + " get " + cre.a.s.name);
			}
		}
	}
}
