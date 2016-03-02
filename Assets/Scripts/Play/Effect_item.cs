//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play.Eff {
	public class UseItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public UseItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
			c_ent = ent;
			c_sel = sel;
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
		public override void Do(Ctx ctx) {
		}
	}

	public class DelItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public DelItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
			c_ent = ent;
			c_sel = sel;
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
		public override void Do(Ctx ctx) {
		}
	}

	public class AddItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemCreate> c_cre;

		public AddItem(Calc<Entity> ent, Calc<ItemCreate> cre) {
			c_ent = ent;
			c_cre = cre;
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
		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Inv inv = ent.GetAttr<Inv>();
			ItemCreate cre = c_cre.Get(ctx);
			List<Item> to = cre.Create(ctx);
			inv.AddItem(to);
		}
	}
}
