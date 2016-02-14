//utf-8。
using System;

namespace Play.Tree {
	public class IaPick : Iact {
		public IaPick(int time1, int time2, int sta, Tree.Stat.ID st, Schema.Tree.Part part, int count)
			: base(
				time1: time1,
				time2: time2,
				eff: new Effect[] {
					new EffDecStat<Tree.Stat.ID> (new Dst(), st, count),
					new EffDecStat<Creature.Stat.ID> (new Src(), Creature.Stat.ID.Stamina, sta),
					new Play.EffAddItem (
						ent : new Src(),
						item : new CalcPartItem(
							ent : new Play.Dst(),
							part : part
						),
						count : new Play.Const<int>(count)
					)
				}
			) {
		}
	}
}
