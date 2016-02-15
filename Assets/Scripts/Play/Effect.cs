//utf-8。
using System.Collections.Generic;

namespace Play {
	public interface Effect {
		bool Can(Ctx ctx);
		void Do(Ctx ctx);
	}

	public class EffMulti : Effect {
		public Effect[] eff;
		public EffMulti(Effect[] eff) {
			this.eff = eff;
		}
		public bool Can(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (!ef.Can(ctx))
					return false;
			}
			return true;
		}

		public void Do(Ctx ctx) {
			foreach (Effect ef in eff) {
				ef.Do(ctx);
			}
		}
	}

	public class EffDecStat<StatID> : Effect where StatID: struct {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_value;

		public EffDecStat(Calc<Entity> ent, StatID id, Calc<int> value) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			if (stat == null)
				return false;
			if (!stat.Has(id))
				return false;
			if (!c_value.Can(ctx))
				return false;
			int value = c_value.Get(ctx);
			if (stat.Get(id) < value)
				return false;
			return true;
		}

		public void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			int value = c_value.Get(ctx);
			stat.ints[id] -= value;
		}
	}

	public class EffUseItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public EffUseItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
			c_ent = ent;
			c_sel = sel;
		}

		public bool Can(Ctx ctx) {
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
		public void Do(Ctx ctx) {
		}
	}

	public class EffDelItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public EffDelItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
			c_ent = ent;
			c_sel = sel;
		}

		public bool Can(Ctx ctx) {
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
		public void Do(Ctx ctx) {
		}
	}

	public class EffAddItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemCreate> c_cre;

		public EffAddItem(Calc<Entity> ent, Calc<ItemCreate> cre) {
			c_ent = ent;
			c_cre = cre;
		}

		public bool Can(Ctx ctx) {
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
		public void Do(Ctx ctx) {
			//TODO
			Entity ent = c_ent.Get(ctx);
			Inv inv = ent.GetAttr<Inv>();
			ItemCreate cre = c_cre.Get(ctx);
			List<Item> to = cre.Create(ctx);
			inv.AddItem(to);
		}
	}
}
