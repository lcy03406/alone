//utf-8。

using System.Collections.Generic;

namespace Schema {
	public static class Ef {
		public static Play.Effect Rest(int sta) {
			return new Play.Eff.IncStat<Play.Stats.Creature>(new Play.Calcs.Src(),
					Play.Stats.Creature.Stamina, new Play.Calcs.Const<int>(sta));
		}

		public static Play.Effect Attack(int sta, Play.Calc<int> damage) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat<Play.Stats.Creature> (new Play.Calcs.Src(),
					Play.Stats.Creature.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.DecStat<Play.Stats.Creature> (new Play.Calcs.Dst(),
					Play.Stats.Creature.HitPoint, damage),
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Pick(int sta, Schema.PartID part, int count) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecPart(ent: new Play.Calcs.Dst(),
					id: part,
					value: new Play.Calcs.Const<int>(count)
				),
				new Play.Eff.DecStat<Play.Stats.Creature> (ent: new Play.Calcs.Src(),
					id: Play.Stats.Creature.Stamina,
					value: new Play.Calcs.Const<int>(sta)),
				new Play.Eff.AddItem ( ent: new Play.Calcs.Src(),
					cre: new Play.Calcs.ItemCount(
						cre: new Play.Calcs.Part(
							ent: new Play.Calcs.Dst(),
							id: part
						),
						count: new Play.Calcs.Const<int>(count)
					)
				)
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Butcher(int sta, Schema.PartID part, int count) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.UseItem(ent: new Play.Calcs.Src(),
					sel: new Play.Calcs.Const<Play.ItemSelect>(
						new Play.ItemSelect(
							a: Schema.Item.GetA(Schema.Item.ID.Knife),
							count: 1
						)
					)
				),
				new Play.Eff.DecPart(ent: new Play.Calcs.Dst(),
					id: part,
					value: new Play.Calcs.Const<int>(count)
				),
				new Play.Eff.DecStat<Play.Stats.Creature> (ent: new Play.Calcs.Src(),
					id: Play.Stats.Creature.Stamina,
					value: new Play.Calcs.Const<int>(sta)),
				new Play.Eff.AddItem ( ent: new Play.Calcs.Src(),
					cre: new Play.Calcs.ItemCount(
						cre: new Play.Calcs.Part(
							ent: new Play.Calcs.Dst(),
							id: part
						),
						count: new Play.Calcs.Const<int>(count)
					)
				)
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Make(int sta,
			Play.ItemSelect[] tools,
			Play.ItemSelect[] reagents,
			Play.ItemCreate[] products,
			Play.EntityCreate build) {
			List<Play.Effect> eff = new List<Play.Effect>();
			if (sta > 0) {
				eff.Add(new Play.Eff.DecStat<Play.Stats.Creature>(new Play.Calcs.Src(),
					Play.Stats.Creature.Stamina, new Play.Calcs.Const<int>(sta)));
			}
			if (tools != null) {
				foreach (Play.ItemSelect sel in tools) {
					eff.Add(new Play.Eff.UseItem(new Play.Calcs.Src(),
						new Play.Calcs.Const<Play.ItemSelect>(sel)));
				}
			}
			if (reagents != null) {
				foreach (Play.ItemSelect sel in reagents) {
					eff.Add(new Play.Eff.DelItem(new Play.Calcs.Src(),
						new Play.Calcs.Const<Play.ItemSelect>(sel)));
				}
			}
			if (products != null) {
				foreach (Play.ItemCreate cre in products) {
					eff.Add(new Play.Eff.AddItem(new Play.Calcs.Src(),
						new Play.Calcs.Const<Play.ItemCreate>(cre)));
				}
			}
			if (build != null) {
				eff.Add(new Play.Eff.AddEntity(new Play.Calcs.Const<Play.EntityCreate>(build)));
			}
			return new Play.Eff.Multi(eff.ToArray());
		}

		public static Play.Effect DelOnZeroPart(PartID[] parts) {
			Play.Calcs.Const<int> intzero = new Play.Calcs.Const<int>(0);
			List<Play.Effect> eff = new List<Play.Effect>();
			foreach (PartID part in parts) {
				eff.Add(new Play.Eff.UsePart(new Play.Calcs.Src(), part, null, intzero));
			}
			eff.Add(new Play.Eff.DelEntity(new Play.Calcs.Src()));
			return new Play.Eff.Multi(eff.ToArray());
		}

		public static Play.Effect DieOnZeroStat<StatID>(StatID[] stats, Stage.A to)
			where StatID : struct
		{
			Play.Calcs.Const<int> intzero = new Play.Calcs.Const<int>(0);
			List<Play.Effect> eff = new List<Play.Effect>();
			foreach (StatID stat in stats) {
				eff.Add(new Play.Eff.UseStat<StatID>(new Play.Calcs.Src(), stat, null, intzero));
			}
			eff.Add(new Play.Eff.ToStage(new Play.Calcs.Src(), to));
			return new Play.Eff.Multi(eff.ToArray());
		}
	}
}
