//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Attrs {
	[Serializable]
	public class Actor : Attrib {
		public Act act = null;

		public override void OnLoad() {
			if (act != null) {
				if (!act.OnLoad(ent)) {
					act = null;
				}
			}
		}

		public override int GetNextTick() {
			if (next_tick > 0)
				return next_tick;
			AI ai = ent.GetAttr<AI>();
			if (act == null && ai != null) {
				Act t = ai.Deque();
				if (t != null) {
					act = t;
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
				}
			}
			if (act == null)
				return;
			ent.layer.world.view.OnEntityAct(ent);
            int next_time = act.NextStep(ent, logs);
			if (next_time < 0) {
				ClrNextTick();
				act = null;
			} else {
				SetNextTick(time + next_time);
			}
		}
	}
}
