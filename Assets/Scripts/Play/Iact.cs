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
				if (distance >= 0) {
					Attrs.Pos sp = ctx.src.GetAttr<Attrs.Pos>();
					Attrs.Pos dp = ctx.dst.GetAttr<Attrs.Pos>();
					if (sp == null || dp == null)
						return false;
					if (sp.c.Manhattan(dp.c) > distance)
						return false;
				}
			}
			return ef.Can(ctx);
		}
		public void Do (Ctx ctx) {
			if (Can(ctx)) {
				ef.Do(ctx);
				ctx.Do();
			}
		}
	}
	
	public class Iacts {
		public static Iact Rest(int time1, int sta) {
			Effect[] eff = new Effect[] {
				new Eff.IncStat<Stats.Creature> (new Calcs.Src(), Stats.Creature.Stamina, new Calcs.Const<int>(sta)),
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
				new Eff.DecStat<Stats.Creature> (new Calcs.Src(), Stats.Creature.Stamina, new Calcs.Const<int>(sta)),
				new Eff.DecStat<Stats.Creature> (new Calcs.Dst(), Stats.Creature.HitPoint, damage),
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
			Schema.PartID part, int count)
		{
			Effect[] eff = new Effect[] {
				new Eff.DecPart(ent: new Calcs.Dst(),
					id: part,
					value: new Calcs.Const<int>(count)
				),
				new Eff.DecStat<Stats.Creature> (ent: new Calcs.Src(),
					id: Stats.Creature.Stamina,
					value: new Calcs.Const<int>(sta)),
				new Eff.AddItem ( ent: new Calcs.Src(),
					cre: new Calcs.ItemCount(
						cre: new Calcs.Part(
							ent: new Calcs.Dst(),
							id: part
						),
						count: new Calcs.Const<int>(count)
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

		public static Iact Butcher(int time1, int time2, int sta,
			Schema.PartID part, int count)
		{
			Effect[] eff = new Effect[] {
				new Eff.UseStage(ent: new Calcs.Dst(),
					stage: typeof(Attrs.Stages.Creature.Dead)
				),
				new Eff.UseItem(ent: new Calcs.Src(),
					sel: new Calcs.Const<ItemSelect>(
						new ItemSelect(
							a: Schema.Item.GetA(Schema.Item.ID.Knife),
							count: 1
						)
					)
				),
				new Eff.DecPart(ent: new Calcs.Dst(),
					id: part,
					value: new Calcs.Const<int>(count)
				),
				new Eff.DecStat<Stats.Creature> (ent: new Calcs.Src(),
					id: Stats.Creature.Stamina,
					value: new Calcs.Const<int>(sta)),
				new Eff.AddItem ( ent: new Calcs.Src(),
					cre: new Calcs.ItemCount(
						cre: new Calcs.Part(
							ent: new Calcs.Dst(),
							id: part
						),
						count: new Calcs.Const<int>(count)
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
			ItemCreate[] products,
			EntityCreate build)
		{
			List<Effect> eff = new List<Effect>();
			if (sta > 0) {
				eff.Add(new Eff.DecStat<Stats.Creature>(new Calcs.Src(), Stats.Creature.Stamina, new Calcs.Const<int>(sta)));
			}
			if (tools != null) {
				foreach (ItemSelect sel in tools) {
					eff.Add(new Eff.UseItem(new Calcs.Src(), new Calcs.Const<ItemSelect>(sel)));
				}
			}
			if (reagents != null) {
				foreach (ItemSelect sel in reagents) {
					eff.Add(new Eff.DelItem(new Calcs.Src(), new Calcs.Const<ItemSelect>(sel)));
				}
			}
			if (products != null) {
				foreach (ItemCreate cre in products) {
					eff.Add(new Eff.AddItem(new Calcs.Src(), new Calcs.Const<ItemCreate>(cre)));
				}
			}
			if (build != null) {
				eff.Add(new Eff.AddEntity(new Calcs.Const<EntityCreate>(build)));
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
