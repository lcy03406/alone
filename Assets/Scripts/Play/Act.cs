//utf-8。
using System;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public abstract class Act {
		public interface Step {
			void Do(Entity ent);
			int Time(Entity ent);
		}
		public abstract string GetName();
		public abstract bool Can(Entity ent);
		public abstract Step GetStep(int i);
		static protected Step GetStep(int i, Step[] steps) {
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

namespace Play.Acts {
	[Serializable]
	public class ActIact : Act {
		public Schema.Iact.A a;
		public WUID dst;
		public Coord dstc;
		public ActIact (Schema.Iact.A iact, WUID dst) {
			this.a = iact;
			this.dst = dst;
		}
		public ActIact(Schema.Iact.A iact, Coord dstc) {
			this.a = iact;
			this.dstc = dstc;
		}
		public override string GetName() {
			return a.s.name;
		}
		private Ctx GetCtx(Entity ent) {
			Ctx ctx = null;
			Entity ent_dst = ent.layer.FindEntity(dst);
			if (ent_dst == null) {
				ctx = new Ctx(ent.layer, ent, null, dstc);
			} else {
				ctx = new Ctx(ent.layer, ent, ent_dst);
			}
			return ctx;
		}
        public override bool Can (Entity ent) {
			Ctx ctx = GetCtx(ent);
			return a.Can (ctx);
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
			}
			int Act.Step.Time (Entity ent) {
				ActIact act = (ActIact)Act.EntAct(ent);
				return act.a.s.time1;
			}
		}
		private class Step2 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActIact act = (ActIact) Act.EntAct (ent);
				Ctx ctx = act.GetCtx(ent);
				act.a.Do (ctx);
			}
			int Act.Step.Time (Entity ent) {
				ActIact act = (ActIact)Act.EntAct(ent);
				return act.a.s.time2;
			}
		}
		private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}
}
