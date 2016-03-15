//utf-8。

using System.Collections.Generic;

namespace Schema {
	public static class Ef {
		public static Play.Effect Rest(int sta) {
			return new Play.Eff.IncStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta));
		}

		public static Play.Effect Move(int sta) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.Move(new Play.Calcs.Src()),
			};
			return new Play.Eff.Multi(eff);
		}

		public static Play.Effect Attack(int sta, int mulDamage, int addDamage) {
			Play.Effect[] eff = new Play.Effect[] {
				new Play.Eff.DecStat(new Play.Calcs.Src(),
					StatID.Stamina, new Play.Calcs.Const<int>(sta)),
				new Play.Eff.DecStat(new Play.Calcs.Dst(),
					StatID.HitPoint, new Play.Calcs.Damage(new Play.Calcs.Src(), StatID.Attack, mulDamage, addDamage))
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

		public static Play.ItemSelect[] MakeItemSelect(List<SomeItem> some) {
			List<Play.ItemSelect> list = new List<Play.ItemSelect>();
			foreach (SomeItem one in some) {
				list.Add(new Play.ItemSelect(Item.GetA(one.id), one.count));
			}
			return list.ToArray();
		}

		public static Play.ItemSelect[] MakeItemSelect(List<SomeItemSelect> some) {
			List<Play.ItemSelect> list = new List<Play.ItemSelect>();
			foreach (SomeItemSelect one in some) {
				List<Item.A> items = new List<SchemaBase<ItemID, Item>.A>();
				foreach (ItemID item in one.items) {
					items.Add(Item.GetA(item));
				}
				Dictionary<UsageID, int> usages = new Dictionary<UsageID, int>();
				foreach (SomeUsage usage in one.usages) {
					usages.Add(usage.id, usage.level);
				}
				list.Add(new Play.ItemSelect(items, usages, one.count));
			}
			return list.ToArray();
		}

		public static Play.ItemCreate[] MakeItemCreate(List<SomeItem> some) {
			List<Play.ItemCreate> list = new List<Play.ItemCreate>();
			foreach (SomeItem one in some) {
				list.Add(new Play.ItemCreate(Item.GetA(one.id), one.count));
			}
			return list.ToArray();
		}

		public static List<Play.Effect> MakeCommon(int sta,
			Play.ItemSelect[] tools,
			Play.ItemSelect[] reagents,
			Play.ItemCreate[] products)
		{
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
			return eff;
		}

		public static List<Play.Effect> MakeCommon(int sta,
				List<SomeItem> tools,
				List<SomeItem> reagents,
				List<SomeItem> products) {
			return MakeCommon(sta, MakeItemSelect(tools), MakeItemSelect(reagents), MakeItemCreate(products));
		}
		public static List<Play.Effect> MakeCommon(int sta,
				List<SomeItemSelect> tools,
				List<SomeItemSelect> reagents,
				List<SomeItem> products) {
			return MakeCommon(sta, MakeItemSelect(tools), MakeItemSelect(reagents), MakeItemCreate(products));
		}


		public static Play.Effect Build(List<Play.Effect> eff, Schema.Entity.A build) {
			eff.Add(new Play.Eff.AddEntity(new Play.Calcs.Const<Schema.Entity.A>(build)));
			return new Play.Eff.Multi(eff.ToArray());
		}

		public static Play.Effect Make(List<Play.Effect> eff) {
			return new Play.Eff.Multi(eff.ToArray());
		}

		public static Play.Effect Pick(List<Play.Effect> eff, Schema.PartID part, int count) {
			eff.Add(new Play.Eff.DecPart(ent: new Play.Calcs.Dst(),
				id: part,
				value: new Play.Calcs.Const<int>(count)
			));
			eff.Add(new Play.Eff.AddItem(ent: new Play.Calcs.Src(),
				cre: new Play.Calcs.ItemCount(
					cre: new Play.Calcs.Part(
						ent: new Play.Calcs.Dst(),
						id: part
					),
					count: new Play.Calcs.Const<int>(count)
				)
			));
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
