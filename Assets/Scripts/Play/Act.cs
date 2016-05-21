//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public abstract class Act {
		public virtual bool OnLoad(Entity ent) { return true; }
		public abstract string GetName();
		public abstract bool Can(Entity ent);
		public abstract int NextStep(Entity ent, List<string> logs); //TODO
	}
}

namespace Play.Acts {
	[Serializable]
	public class ActIact : Act {
		public Schema.Iact.A a;
		public WUID dst_id;
		public Coord dst_c;
		[NonSerialized]	public Entity dst_ent;
		public int istep = -1;
		public ActIact (Schema.Iact.A iact, Entity dst_ent) {
			this.a = iact;
			if (dst_ent != null) {
				this.dst_id = dst_ent.id;
				this.dst_c = dst_ent.GetAttr<Attrs.Pos>().c;
				this.dst_ent = dst_ent;
			}
		}
		public ActIact(Schema.Iact.A iact, Coord dstc) {
			this.a = iact;
			this.dst_c = dstc;
		}
		public override bool OnLoad(Entity ent) {
			if (dst_id != WUID.None) {
				this.dst_ent = ent.layer.FindEntity(dst_id);
				return dst_ent != null;
			}
			return true;
		}
		public override string GetName() {
			return a.s.name;
		}
		private Ctx GetCtx(Entity ent) {
			return new Ctx(ent.layer, ent, dst_ent, dst_c);
		}
        public override bool Can (Entity ent) {
			Ctx ctx = GetCtx(ent);
			return a.Can (ctx);
		}
		private interface Step {
			void Do(Entity ent, List<string> logs);
			int Time(Entity ent);
		}
		private class Step1 : Step {
			void Step.Do (Entity ent, List<string> logs) {
			}
			int Step.Time (Entity ent) {
				ActIact act = (ActIact)EntAct(ent);
				return act.a.s.time1;
			}
		}
		private class Step2 : Step {
			void Step.Do (Entity ent, List<string> logs) {
				ActIact act = (ActIact) EntAct (ent);
				Ctx ctx = act.GetCtx(ent);
				act.a.Do (ctx, logs);
			}
			int Step.Time (Entity ent) {
				ActIact act = (ActIact) EntAct(ent);
				return act.a.s.time2;
			}
		}
		private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
		public override int NextStep (Entity ent, List<string> logs) {
			istep++;
			Step step = GetStep (istep, steps);
			if (step == null)
				return -1;
			step.Do(ent, logs);
			return step.Time(ent);
		}
		static private Step GetStep(int i, Step[] steps) {
			if (i >= 0 && i < steps.Length)
				return steps[i];
			return null;
		}
		static public Act EntAct(Entity ent) {
			Attrs.Actor actor = ent.GetAttr<Attrs.Actor>();
			return actor.act;
		}
	}
}
