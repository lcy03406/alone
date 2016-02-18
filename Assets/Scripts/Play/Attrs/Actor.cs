//utf-8。
using System;

namespace Play.Attrs {
	[Serializable]
	public class Actor : Attrib {
		public int acstep = 0;
		public int actime = 0;
		public Act act = null;

		public virtual int NextTick () {
			return actime;
		}
		public virtual void Tick (int time) {
			while (time >= actime) {
				AI ai = ent.GetAttr<AI>();
				if (act == null && ai != null) {
					Act t = ai.NextAct();
					if (t != null && t.Can(ent)) {
						this.act = t;
						acstep = -1;
					}
				}
				if (act == null)
					break;
				acstep++;
				Act.Step step = act.GetStep(acstep);
				if (step == null) {
					act = null;
				} else {
					actime = time + step.Time(ent);
					step.Do(ent);
				}
			}
		}
	}
}
