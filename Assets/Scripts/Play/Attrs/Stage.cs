//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public sealed class Stage : Attrib {
		public Schema.Stage.A a;
		public int start_time = 0;
		public int tick_time = 0;
		public void Transit(Schema.Stage.A to) {
			if (ent != null && ent.layer != null) {
				Ctx ctx = new Ctx(ent.layer, ent);
				Effect ef = a.s.finish_ef;
				if (ef != null && ef.Can(ctx))
					ef.Do(ctx);
			}
			a = to;
			start_time = tick_time;
			if (ent != null && ent.layer != null) {
				Ctx ctx = new Ctx(ent.layer, ent);
				Effect ef = a.s.start_ef;
				if (ef != null && ef.Can(ctx))
					ef.Do(ctx);
			}
		}

		public int NextTick() {
			return int.MaxValue;
		}

		public void Tick(int time) {
			tick_time = time;
			Ctx ctx = new Ctx(ent.layer, ent);
			Effect ef = a.s.tick_ef;
			if (ef != null && ef.Can(ctx))
				ef.Do(ctx);
		}
	}
}
/*
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
*/
