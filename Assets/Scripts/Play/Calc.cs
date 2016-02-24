//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play {
	public sealed class Ctx {
		public class Invx {
			public List<Item> use = new List<Item>();
			public List<Item> del = new List<Item>();
		}
		public World world = null;
		public Entity src = null;
		public Entity dst = null;
		public List<Entity> use_ent = new List<Entity>();
		public Dictionary<Inv, Invx> invs = new Dictionary<Inv, Invx>();
		public List<List<Item>> items = new List<List<Item>>();

		public Ctx(World world, Entity src, Entity dst) {
			this.world = world;
			this.src = src;
			this.dst = dst;
		}

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

/*	public class Rand<T> : Calc<T> {
		public Dictionary<Calc<T>, int> choices;
		public Rand(Dictionary<Calc<T>, int> choices) {
			this.choices = choices;
		}
		public bool Can(Ctx ctx) {
			foreach (Calc<T> key in choices.Keys) {
				if (!key.Can(ctx)) {
					return false;
				}
			}
            return true;
		}
		public T Get(Ctx ctx) {
			int total = 0;
			foreach (int value in choices.Values) {
				total += value;
			}
			int rand = ctx.world.rand.Next(0, total);
			foreach (KeyValuePair<Calc<T>, int> pair in choices) {
				rand -= pair.Value;
				if (rand < 0)
					return pair.Key.Get(ctx);
			}
			Assert.IsTrue(false);
			return default(T);
		}
	}*/

	public class RandConst<T> : Calc<T> {
		public List<Choice<T>> choices;
        public RandConst(List<Choice<T>> choices) {
			this.choices = choices;
		}
		public bool Can(Ctx ctx) {
			return true;
		}
		public T Get(Ctx ctx) {
			return Choice<T>.Choose(choices);
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

	public class GetStat<StatID> : Calc<int> where StatID : struct {
		public readonly Calc<Entity> c_ent;
		public readonly StatID id;
		public GetStat(Calc<Entity> ent, StatID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			if (stat == null)
				return false;
			return true;
		}

		public int Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat<StatID> stat = ent.GetAttr<Stat<StatID>>();
			return stat.Get(id);
		}
	}

	public class Part : Calc<ItemCreate> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.PartID id;
		public Part(Calc<Entity> ent, Schema.PartID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Grow grow = ent.GetAttr<Grow>();
			if (grow == null)
				return false;
			Grow.Part part = grow.Get(id);
			if (part == null)
				return false;
			return true;
		}

		public ItemCreate Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Grow grow = ent.GetAttr<Grow>();
			ItemCreate cre = grow.Get(id).GetItem();
			return cre;
		}
	}

	public class ItemCount : Calc<ItemCreate> {
		Calc<ItemCreate> c_cre;
		Calc<int> c_count;

		public ItemCount(Calc<ItemCreate> cre, Calc<int> count) {
			c_cre = cre;
			c_count = count;
		}

		public bool Can(Ctx ctx) {
			return c_cre.Can(ctx) && c_count.Can(ctx);
		}

		public ItemCreate Get(Ctx ctx) {
			ItemCreate cre = c_cre.Get(ctx);
			cre.count = c_count.Get(ctx);
			return cre;
		}
	}
}
