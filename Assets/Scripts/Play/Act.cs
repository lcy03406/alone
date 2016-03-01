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
	public class ActMove : Act {
		public Direction to;
		public ActMove (Direction to) {
			this.to = to;
		}
		public override bool Can (Entity ent) {
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			if (pos == null)
				return false;
			return ent.world.CanMoveTo (pos.c.Step (to));
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActMove act = (ActMove) Act.EntAct (ent);
				Direction to = act.to;
				Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
				pos.dir = to;
			}
			int Act.Step.Time (Entity ent) {
				return 5;
			}
		}
		private class Step2 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActMove act = (ActMove) Act.EntAct (ent);
				Direction to = act.to;
				Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
				Coord tc = pos.c.Step (to);
				World world = ent.world;
				if (world.CanMoveTo (tc)) {
					world.MoveEntity (ent, tc);
				}
			}
			int Act.Step.Time (Entity ent) {
				return 5;
			}
		}
		private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}

	[Serializable]
	public class ActDir : Act {
		public Direction to;
		public ActDir (Direction to) {
			this.to = to;
		}
		public override bool Can (Entity ent) {
			return true;
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActDir act = (ActDir) Act.EntAct (ent);
				Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
				pos.dir = act.to;
			}
			int Act.Step.Time (Entity ent) {
				return 0;
			}
		}
		private static Step[] steps = new Step[] { new Step1 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}

	[Serializable]
	public class ActIact : Act {
		public Schema.Iact.A a;
		public WUID dst;
		public ActIact (Schema.Iact.A iact, WUID dst) {
			this.a = iact;
			this.dst = dst;
		}
		public override bool Can (Entity ent) {
			Entity ent_dst = ent.world.FindEntity(dst);
            Ctx ctx = new Ctx(ent.world, ent, ent_dst);
			Iact iact = new Iact(a);
			return iact.Can (ctx);
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
				Entity ent_dst = ent.world.FindEntity(act.dst);
				Ctx ctx = new Ctx(ent.world, ent, ent_dst);
				Iact iact = new Iact(act.a);
				iact.Do (ctx);
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