//utf-8。
using System.Collections.Generic;

namespace Play {
	public class Iact {
		public int time1;
		public int time2;
		public bool has_dst;
		public int distance;
		public Effect ef;
		public Iact(int time1, int time2, bool has_dst, int distance, Effect[] eff) {
			this.time1 = time1;
			this.time2 = time2;
			this.has_dst = has_dst;
			this.distance = distance;
			this.ef = new Eff.Multi(eff);
		}
		public bool Can (Ctx ctx) {
			if (has_dst) {
				if (ctx.dst == null)
					return false;
				if (ctx.src.c.Manhattan(ctx.dst.c) > distance)
					return false;
			}
			return ef.Can(ctx);
		}
		public void Do (Ctx ctx) {
			if (Can(ctx))
				ef.Do(ctx);
		}
	}
	
	public class Iacts {
		public static Iact Rest(int time1, int sta) {
			Effect[] eff = new Effect[] {
				new Eff.IncStat<Creature.Stat.ID> (new Calcs.Src(), Creature.Stat.ID.Stamina, new Calcs.Const<int>(sta)),
			};
			return new Iact(
				time1: time1,
				time2: 0,
				has_dst: false,
				distance: 0,
				eff: eff
			);
		}

		public static Iact Attack(int time1, int time2, int sta,
			Calc<int> damage) {
			Effect[] eff = new Effect[] {
				new Eff.DecStat<Creature.Stat.ID> (new Calcs.Src(), Creature.Stat.ID.Stamina, new Calcs.Const<int>(sta)),
				new Eff.DecStat<Creature.Stat.ID> (new Calcs.Dst(), Creature.Stat.ID.HitPoint, damage),
			};
			return new Iact(
				time1: time1,
				time2: time2,
				has_dst: true,
				distance: 1,
				eff: eff
			);
		}

		public static Iact Pick(int time1, int time2, int sta,
			Tree.Stat.ID st, Schema.Tree.Part part, int count)
		{
			Effect[] eff = new Effect[] {
				new Eff.DecStat<Tree.Stat.ID> (new Calcs.Dst(), st, new Calcs.Const<int>(count)),
				new Eff.DecStat<Creature.Stat.ID> (new Calcs.Src(), Creature.Stat.ID.Stamina, new Calcs.Const<int>(sta)),
				new Play.Eff.AddItem (
					ent: new Calcs.Src(),
					cre: new Calcs.TreePart(
						ent: new Calcs.Dst(),
						part: part
					)
				)
			};
            return new Iact(
				time1: time1,
				time2: time2,
				has_dst: true,
				distance: 1,
				eff: eff
			);
		}

		public static Iact Make(int time1, int time2, int sta,
			ItemSelect[] tools,
			ItemSelect[] reagents,
			ItemCreate[] products)
		{
			List<Effect> eff = new List<Effect>();
			eff.Add(new Eff.DecStat<Creature.Stat.ID>(new Calcs.Src(), Creature.Stat.ID.Stamina, new Calcs.Const<int>(sta)));
			foreach (ItemSelect sel in tools) {
				eff.Add(new Eff.UseItem(new Calcs.Src(), new Calcs.Const<ItemSelect>(sel)));
			}
			foreach (ItemSelect sel in reagents) {
				eff.Add(new Eff.DelItem(new Calcs.Src(), new Calcs.Const<ItemSelect>(sel)));
			}
			foreach (ItemCreate cre in products) {
				eff.Add(new Eff.AddItem(new Calcs.Src(), new Calcs.Const<ItemCreate>(cre)));
			}
			return new Iact(
				time1: time1,
				time2: time2,
				has_dst: false,
				distance: 0,
				eff: eff.ToArray()
			);
		}
	}
}