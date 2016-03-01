//utf-8。

using System.Collections.Generic;

namespace Schema {
	public sealed partial class Iact : SchemaBase<Iact.ID, Iact> {
		public readonly string name;
		public readonly int time1;
		public readonly int time2;
		public readonly bool has_dst;
		public readonly int distance;
		public readonly Play.Effect ef;

		private Iact(string name, int time1, int time2, bool has_dst, int distance, Play.Effect[] eff) {
			this.name = name;
			this.time1 = time1;
			this.time2 = time2;
			this.has_dst = has_dst;
			this.distance = distance;
			this.ef = new Play.Eff.Multi(eff);
		}

		private static Iact Rest(string name, int time1, int sta) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.IncStat<Play.Stats.Creature> (new Play.Calcs.Src(),
					Play.Stats.Creature.Stamina, new Play.Calcs.Const<int>(sta)),
			};
			return new Iact(
				name: name,
				time1: time1,
				time2: 0,
				has_dst: false,
				distance: 0,
				eff: eff
			);
		}

		public static Iact Attack(string name, int time1, int time2, int sta,
			Play.Calc<int> damage) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat<Play.Stats.Creature> (new Play.Calcs.Src(),
					Play.Stats.Creature.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.DecStat<Play.Stats.Creature> (new Play.Calcs.Dst(),
					Play.Stats.Creature.HitPoint, damage),
			};
			return new Iact(
				name: name,
				time1: time1,
				time2: time2,
				has_dst: true,
				distance: 1,
				eff: eff
			);
		}

		public static Iact Pick(string name, int time1, int time2, int sta,
			Schema.PartID part, int count) {
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
			return new Iact(
				name: name,
				time1: time1,
				time2: time2,
				has_dst: true,
				distance: 1,
				eff: eff
			);
		}

		public static Iact Butcher(string name, int time1, int time2, int sta,
			Schema.PartID part, int count) {
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
			return new Iact(
				name: name,
				time1: time1,
				time2: time2,
				has_dst: true,
				distance: 1,
				eff: eff
			);
		}

		public static Iact Make(string name, int time1, int time2, int sta,
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
			return new Iact(
				name: name,
				time1: time1,
				time2: time2,
				has_dst: false,
				distance: 0,
				eff: eff.ToArray()
			);
		}
	}
}
