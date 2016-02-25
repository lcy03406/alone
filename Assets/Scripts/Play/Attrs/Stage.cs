//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public abstract class Stage : Attrib {
		public int start_time = 0;
		public void Transit(Stage to, int time) {
			to.start_time = time;
			ent.SetAttr(to);
		}
		public override void SetEntity(Entity ent) {
			Entity old_ent = this.ent;
			if (old_ent == null && ent != null) {
				base.SetEntity(ent);
				int time = ent.world.param.time;
				Start(time);
			} else if (old_ent != null && ent == null) {
				int time = old_ent.world.param.time;
				Stop(time);
				base.SetEntity(ent);
			} else {
				Assert.IsTrue(old_ent == ent);
            }
		}
		public virtual void Start(int time) { }
		public virtual void Tick(int time) { }
		public virtual void Stop(int time) { }
	}
}

namespace Play.Attrs.Stages.Static {
	[Serializable]
	public class Static : Stage {
	}
}

namespace Play.Attrs.Stages.Tree {
	[Serializable]
	public class Young : Stage {
		int tick_time = 0;
		public override void Start(int time) {
			tick_time = start_time;
		}
		public override void Tick(int time) {
			int t = time - tick_time;
			Stat<Stats.Tree> stat = ent.GetAttr<Stat<Stats.Tree>>();
			int grouth = stat.Get(Stats.Tree.Grouth);
			int cap = stat.Cap(Stats.Tree.Grouth);
			int need = cap - grouth;
			int grow = need > t ? t : need;
			stat.Set(Stats.Tree.Grouth, grouth + grow);
			tick_time += grow;
			if (grow == need) {
				Transit(new Grown(), tick_time);
			}
		}
	}

	[Serializable]
	public class Grown : Stage {
	}
}

namespace Play.Attrs.Stages.Creature {
	[Serializable]
	public class Alive : Stage {
		public override void Tick(int time) {
			Stat<Stats.Creature> stat = ent.GetAttr<Stat<Stats.Creature>>();
			int hp = stat.Get(Stats.Creature.HitPoint);
			if (hp <= 0) {
				Transit(new Dead(), time);
			}
		}
	}

	[Serializable]
	public class Dead : Stage {
		public override void Start(int time) {
			ent.DelAttr<Actor>();
		}
	}
}

namespace Play.Attrs.Stages.Workshop {
	[Serializable]
	public class Off : Stage {
		public override void Tick(int time) {
			Transit(new On(), time);
		}
	}

	[Serializable]
	public class On : Stage {
	}
}
