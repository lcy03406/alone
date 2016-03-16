//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Attrs {
	[Serializable]
	public class Actor : Attrib {
		public int acstep = 0;
		public Act act = null;

		public override void OnBorn() {
			base.OnBorn();
		}

		public override int GetNextTick() {
			if (next_tick > 0)
				return next_tick;
			AI ai = ent.GetAttr<AI>();
			if (act == null && ai != null) {
				Act t = ai.Deque();
				if (t != null) {
					act = t;
					acstep = -1;
					SetNextTick(ent.layer.world.param.time);
				}
			}
			return next_tick;
		}

		public override void Tick (int time, List<string> logs) {
			AI ai = ent.GetAttr<AI>();
			if (act == null && ai != null) {
				Act t = ai.Deque();
				if (t != null && t.Can(ent)) {
					act = t;
					acstep = -1;
				}
			}
			if (act == null)
				return;
			acstep++;
			Act.Step step = act.GetStep(acstep);
			if (step == null) {
				act = null;
			} else {
				SetNextTick(time + step.Time(ent));
				step.Do(ent, logs);
			}
		}
	}
}
