using System;

namespace Schema {
	public sealed class Stage : SchemaBase<Stage.ID, Stage> {
		public readonly string name;
		public readonly Play.Effect start_ef;
		public readonly Play.Effect tick_ef;
		public readonly Play.Effect finish_ef;

		public Stage(string name, 
			Play.Effect start_ef, Play.Effect tick_ef, Play.Effect finish_ef) {
			this.name = name;
			this.start_ef = start_ef;
			this.tick_ef = tick_ef;
			this.finish_ef = finish_ef;
		}

		public enum ID {
			Boulder_Static,
			Tree_Young,
			Tree_Grown,
			Creature_Alive,
			Creature_Dead,
			Workshop_Off,
			Workshop_On,
		}
		static public void Init () {
			Add(ID.Boulder_Static, new Stage(name: "static",
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(new PartID[] { PartID.Boulder_Stone }),
				finish_ef: null
			));
			PartID[] tree_part = new PartID[] {
				PartID.Tree_Branch,
				//PartID.Tree_Bough,
				PartID.Tree_Fruit,
				//PartID.Tree_Bark,
			};
            Add(ID.Tree_Young, new Stage(name: "young",
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(tree_part), //TODO
				finish_ef: null
			));
			Add(ID.Tree_Grown, new Stage(name: "grown",
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(tree_part),
				finish_ef: null
			));
			PartID[] creature_part = new PartID[] {
				PartID.Creature_Skin,
				PartID.Creature_Meat,
				PartID.Creature_Bone,
			};
			Add(ID.Creature_Alive, new Stage(name: "alive",
				start_ef: null,
				tick_ef: Ef.DieOnZeroStat(
					new StatID[] { StatID.Creature_HitPoint },
					Stage.GetA(Stage.ID.Creature_Dead)),
				finish_ef: null
			));
			Add(ID.Creature_Dead, new Stage(name: "dead",
				start_ef: null,
				tick_ef: Ef.DelOnZeroPart(creature_part),
				finish_ef: null
			));
			Add(ID.Workshop_Off, new Stage(name: "off",
				start_ef: null,
				tick_ef: null,
				finish_ef: null
			));
			Add(ID.Workshop_On, new Stage(name: "on",
				start_ef: null,
				tick_ef: null,
				finish_ef: null
			));
		}
	}
}
