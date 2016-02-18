//utf-8。
using System.Collections.Generic;
using Play.Attrs;

namespace Play {
	public interface Effect {
		bool Can(Ctx ctx);
		void Do(Ctx ctx);
	}
}

namespace Play.Eff {
	public class Multi : Effect {
		public Effect[] eff;
		public Multi(Effect[] eff) {
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

	public class IncStat<StatID> : Effect where StatID : struct {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_value;

		public IncStat(Calc<Entity> ent, StatID id, Calc<int> value) {
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
			if (!c_value.Can(ctx))
				return false;
			return true;
		}

		public void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			int value = c_value.Get(ctx);
			stat.Set(id, stat.Get(id) + value);
		}
	}

	public class DecStat<StatID> : Effect where StatID: struct {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_value;

		public DecStat(Calc<Entity> ent, StatID id, Calc<int> value) {
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
			if (!c_value.Can(ctx))
				return false;
			int value = c_value.Get(ctx);
			if (value > 0 && stat.Get(id) < value)
				return false;
			return true;
		}

		public void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			int value = c_value.Get(ctx);
			if (value > 0) {
				stat.Set(id, stat.Get(id) - value);
			}
		}
	}

	public class UseItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public UseItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
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

	public class DelItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemSelect> c_sel;

		public DelItem(Calc<Entity> ent, Calc<ItemSelect> sel) {
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

	public class AddItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<ItemCreate> c_cre;

		public AddItem(Calc<Entity> ent, Calc<ItemCreate> cre) {
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
			Entity ent = c_ent.Get(ctx);
			Inv inv = ent.GetAttr<Inv>();
			ItemCreate cre = c_cre.Get(ctx);
			List<Item> to = cre.Create(ctx);
			inv.AddItem(to);
		}
	}
	public class AddEntity : Effect {
		public readonly Calc<EntityCreate> c_cre;

		public AddEntity(Calc<EntityCreate> cre) {
			c_cre = cre;
		}

		public bool Can(Ctx ctx) {
			Coord c = ctx.src.c.Step(ctx.src.dir);
			if (ctx.world.SearchEntity(c) != null)
				return false;
			if (!c_cre.Can(ctx))
				return false;
			return true;
		}
		public void Do(Ctx ctx) {
			//TODO
			Coord c = ctx.src.c.Step(ctx.src.dir);
			EntityCreate cre = c_cre.Get(ctx);
			Entity e = cre.Create(ctx);
			e.c = c;
			ctx.world.AddEntity(e);
		}
	}
}
