using System;

namespace Schema {
	public sealed class Stage : SchemaBase<Stage.ID, Stage> {
		public readonly string name;
		public readonly RenderOrderID render_order;
		public readonly bool blockade;
		public readonly Play.Effect start_ef;
		public readonly Play.Effect tick_ef;
		public readonly Play.Effect finish_ef;

		public Stage(string name, RenderOrderID render_order, bool blockade,
			Play.Effect start_ef, Play.Effect tick_ef, Play.Effect finish_ef) {
			this.name = name;
			this.render_order = render_order;
			this.blockade = blockade;
			this.start_ef = start_ef;
			this.tick_ef = tick_ef;
			this.finish_ef = finish_ef;
		}

		public enum ID {
			GoDie,
			Item_Static,
			Boulder_Static,
			Tree_Young,
			Tree_Grown,
			Creature_Alive,
			Creature_Dead,
			Workshop_Off,
			Workshop_On,
		}
		static public void Init () {
			Add(ID.GoDie, new Stage(name: "go die",
				render_order: RenderOrderID.Corpse,
				blockade: false,
				start_ef: new Play.Eff.Multi(new Play.Effect[] {
					new Play.Eff.DropAllPart(new Play.Calcs.Src()),
					new Play.Eff.DelEntity(new Play.Calcs.Src()),
				}),
				tick_ef: null,
				finish_ef: null
			));
			Add(ID.Item_Static, new Stage(name: "item",
				render_order: RenderOrderID.Item,
				blockade: true, //TODO
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(new PartID[] { PartID.Item }),
				finish_ef: null
			));
			Add(ID.Boulder_Static, new Stage(name: "boulder",
				render_order: RenderOrderID.Boulder,
				blockade: true,
				start_ef: null,
				tick_ef: new Play.Eff.One(new Play.Effect[] {
					Ef.DelOnZeroPart(new PartID[] { PartID.Boulder_Stone }),
					Ef.DieOnZeroStat(new StatID[] { StatID.HitPoint }, GetA(ID.GoDie)),
				}),
				finish_ef: null
			));
			PartID[] tree_part = new PartID[] {
				PartID.Tree_Branch,
				//PartID.Tree_Bough,
				PartID.Tree_Fruit,
				//PartID.Tree_Bark,
			};
			Play.Effect tree_tick = new Play.Eff.One(new Play.Effect[] {
					Ef.DelOnZeroPart(tree_part),
					Ef.DieOnZeroStat(new StatID[] { StatID.HitPoint }, GetA(ID.GoDie)),
				});
            Add(ID.Tree_Young, new Stage(name: "young",
				render_order: RenderOrderID.Tree,
				blockade: true,
				start_ef: null,
				tick_ef: tree_tick,
				finish_ef: null
			));
			Add(ID.Tree_Grown, new Stage(name: "grown",
				render_order: RenderOrderID.Tree,
				blockade: true,
				start_ef: null,
				tick_ef: tree_tick,
				finish_ef: null
			));
			PartID[] creature_part = new PartID[] {
				PartID.Creature_Skin,
				PartID.Creature_Meat,
				PartID.Creature_Bone,
			};
			Add(ID.Creature_Alive, new Stage(name: "alive",
				render_order: RenderOrderID.Creature,
				blockade: true,
				start_ef: null,
				tick_ef: Ef.DieOnZeroStat(
					new StatID[] { StatID.HitPoint },
					Stage.GetA(Stage.ID.Creature_Dead)),
				finish_ef: null
			));
			Add(ID.Creature_Dead, new Stage(name: "dead",
				render_order: RenderOrderID.Corpse,
				blockade: false,
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(creature_part),
				finish_ef: null
			));
			Add(ID.Workshop_Off, new Stage(name: "off",
				render_order: RenderOrderID.Building,
				blockade: true,
				start_ef: null,
				tick_ef: null,
				finish_ef: null
			));
			Add(ID.Workshop_On, new Stage(name: "on",
				render_order: RenderOrderID.Building,
				blockade: true,
				start_ef: null,
				tick_ef: null,
				finish_ef: null
			));
		}
	}
}
