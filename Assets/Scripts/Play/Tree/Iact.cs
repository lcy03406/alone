//utf-8。
using System;

namespace Play.Tree {
	public class PickBranch : Iact {
		public PickBranch() {
			eff = new Effect[] {
				new DecStat<Tree.Stat.ID> (new Dst(), Tree.Stat.ID.Branch, 1),
                new DecStat<Creature.Stat.ID> (new Src(), Creature.Stat.ID.Stamina, 1),
                new Play.AddItem (
					ent : new Src(),
					item : new CalcPartItem(
						ent : new Play.Dst(),
						part : Schema.Tree.Part.Branch
					),
					count : new Play.Const<int>(1)
				)
			};
		}
	}

	public class PickFruit : Iact {
		public PickFruit() {
			eff = new Effect[] {
				new DecStat<Tree.Stat.ID> (new Dst(), Tree.Stat.ID.Fruit, 1),
				new DecStat<Creature.Stat.ID> (new Src(), Creature.Stat.ID.Stamina, 1),
				new Play.AddItem (
					ent : new Src(),
					item : new CalcPartItem(
						ent : new Play.Dst(),
						part : Schema.Tree.Part.Fruit
					),
					count : new Play.Const<int>(1)
				)
			};
		}
	}
}
