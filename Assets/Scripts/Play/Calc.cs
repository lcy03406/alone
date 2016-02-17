//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	public sealed class Ctx {
		public class Invx {
			public List<Item> use = new List<Item>();
			public List<Item> del = new List<Item>();
		}
		public Entity src = null;
		public Entity dst = null;
		public Dictionary<Inv, Invx> invs = new Dictionary<Inv, Invx>();
		public List<List<Item>> items = new List<List<Item>>();

		public Invx GetInv(Inv inv) {
			Invx invx;
			if (invs.TryGetValue(inv, out invx))
				return invx;
			invx = new Invx();
			invs.Add(inv, invx);
			return invx;
		}

		public void Do() {
			foreach (KeyValuePair<Inv, Invx> pair in invs) {
				pair.Key.DelItem(pair.Value.del);
			}
        }
	}

	public interface Calc<T> {
		bool Can(Ctx ctx);
		T Get(Ctx ctx);
	}
}

namespace Play.Calcs {
	public class Const<T> : Calc<T> {
		public T t;
		public Const(T t) {
			this.t = t;
		}
		public bool Can(Ctx ctx) {
			return true;
		}
		public T Get(Ctx ctx) {
			return t;
		}
	}
	public class Src : Calc<Entity> {
		public bool Can(Ctx ctx) {
			return true;
		}
		public Entity Get(Ctx ctx) {
			return ctx.src;
		}
	}

	public class Dst : Calc<Entity> {
		public bool Can(Ctx ctx) {
			return true;
		}
		public Entity Get(Ctx ctx) {
			return ctx.dst;
		}
	}

	public class Stat<StatID> : Calc<int> where StatID : struct {
		public readonly Calc<Entity> c_ent;
		public readonly StatID id;
		public Stat(Calc<Entity> ent, StatID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Play.Stat<StatID> stat = ent.GetAttr<Play.Stat<StatID>>();
			if (stat == null)
				return false;
			return true;
		}

		public int Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Play.Stat<StatID> stat = ent.GetAttr<Play.Stat<StatID>>();
			return stat.Get(id);
		}
	}

	public class TreePart : Calc<ItemCreate> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.Tree.Part part;
		public TreePart(Calc<Entity> ent, Schema.Tree.Part part) {
			this.c_ent = ent;
			this.part = part;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Tree.Core core = ent.GetAttr<Tree.Core>();
			if (core == null)
				return false;
			return true;
		}

		public ItemCreate Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Tree.Core core = ent.GetAttr<Tree.Core>();
			ItemCreate cre = new ItemCreate(a: core.race.s.items[part],
				q_base: 10, //TODO
				q_from: null,
				q_rand: 0,
				cap_from: null,
				count: 1);
			return cre;
		}
	}
}
