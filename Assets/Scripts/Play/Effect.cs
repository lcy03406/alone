//utf-8。
using System;

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
		int value;

		public EffDecStat(Calc<Entity> ent, StatID id, int value) {
			c_ent = ent;
			this.id = id;
			this.value = value;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			if (stat == null)
				return false;
			if (stat.ints[id] < value)
				return false;
			return true;
		}

		public void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			stat.ints[id] -= value;
		}
	}

	public class EffAddItem : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Calc<Schema.Item.A> c_item;
		public readonly Calc<int> c_count;

		public EffAddItem(Calc<Entity> ent, Calc<Schema.Item.A> item, Calc<int> count) {
			c_ent = ent;
			c_item = item;
			c_count = count;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Inv inv = ent.GetAttr<Inv>();
			if (inv == null)
				return false;
			bool item = c_item.Can(ctx);
			if (!item)
				return false;
			bool count = c_count.Can(ctx);
			if (!count)
				return false;
			return true;
		}
		public void Do(Ctx ctx) {
			//TODO
			Entity ent = c_ent.Get(ctx);
			Inv inv = ent.GetAttr<Inv>();
			Schema.Item.A a = c_item.Get(ctx);
			int count = c_count.Get(ctx);
			ItemCreate cre = new ItemCreate(a: a,
				q_base: 10, q_from: null, q_rand: 0,
				cap_from: null,
				count: count);
			inv.AddItem(cre.Create(ctx));
		}
	}
}
