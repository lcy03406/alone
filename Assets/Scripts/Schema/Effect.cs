//utf-8。

using System.Collections.Generic;

namespace Schema {
	public static class Ef {
		public static Play.Effect Rest(int sta) {
			return new Play.Eff.IncStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta));
		}

		public static Play.Effect Attack(int sta, Play.Calc<int> damage) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.DecStat(new Play.Calcs.Dst(),
					StatID.HitPoint, damage),
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Travel(int sta, int to) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.GoLayer(new Play.Calcs.Src(), to),
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Pick(int sta, Schema.PartID part, int count) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecPart(ent: new Play.Calcs.Dst(),
					id: part,
					value: new Play.Calcs.Const<int>(count)
				),
				new Play.Eff.DecStat(ent: new Play.Calcs.Src(),
					id: StatID.Stamina,
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
/*
		public static Play.Effect Butcher(int sta, Schema.PartID part, int count) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.UseItem(ent: new Play.Calcs.Src(),
					sel: new Play.Calcs.Const<Play.ItemSelect>(
						new Play.ItemSelect(
							a: Schema.Item.GetA(Schema.ItemID.Knife),
							count: 1
						)
					)
				),
				new Play.Eff.DecPart(ent: new Play.Calcs.Dst(),
					id: part,
					value: new Play.Calcs.Const<int>(count)
				),
				new Play.Eff.DecStat(ent: new Play.Calcs.Src(),
					id: StatID.Creature_Stamina,
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
*/
		public static Play.Effect Make(int sta,
			Play.ItemSelect[] tools,
			Play.ItemSelect[] reagents,
			Play.ItemCreate[] products,
			Schema.Entity.A build) {
			List<Play.Effect> eff = new List<Play.Effect>();
			if (sta > 0) {
				eff.Add(new Play.Eff.DecStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta)));
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
				eff.Add(new Play.Eff.AddEntity(new Play.Calcs.Const<Schema.Entity.A>(build)));
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

		public static Play.Effect DieOnZeroStat(Schema.StatID[] stats, Stage.A to)
		{
			Play.Calcs.Const<int> intzero = new Play.Calcs.Const<int>(0);
			List<Play.Effect> eff = new List<Play.Effect>();
			foreach (StatID stat in stats) {
				eff.Add(new Play.Eff.UseStat(new Play.Calcs.Src(), stat, null, intzero));
			}
			eff.Add(new Play.Eff.ToStage(new Play.Calcs.Src(), to));
			return new Play.Eff.Multi(eff.ToArray());
		}
	}
}
