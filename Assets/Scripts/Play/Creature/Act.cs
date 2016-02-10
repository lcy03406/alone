//utf-8。
using System;

namespace Play.Creature {
	[Serializable]
	public abstract class Act {
		public interface Step {
			void Do (Entity ent);
			int Time (Entity ent);
		}
		public abstract bool Can (Entity ent);
		public abstract Step GetStep (int i);
		static protected Step GetStep (int i, Step[] steps) {
			if (i >= 0 && i < steps.Length)
				return steps[i];
			return null;
		}
		static public Act EntAct (Entity ent) {
			Core core = ent.GetAttr<Core> ();
			return core.act;
        }
	}

	[Serializable]
	public class ActWait : Act {
		public ActWait () {
		}
		public override bool Can (Entity ent) {
			return true;
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
			}
			int Act.Step.Time (Entity ent) {
				return 1;
			}
		}
		private static Step[] steps = new Step[] { new Step1 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}

	[Serializable]
	public class ActMove : Act {
		public Direction to;
		public ActMove (Direction to) {
			this.to = to;
		}
		public override bool Can (Entity ent) {
			return ent.world.CanMoveTo (ent.c.Step (to));
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActMove act = (ActMove) Act.EntAct (ent);
				Direction to = act.to;
				ent.dir = to;
			}
			int Act.Step.Time (Entity ent) {
				return 5;
			}
		}
		private class Step2 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActMove act = (ActMove) Act.EntAct (ent);
				Direction to = act.to;
				Coord tc = ent.c.Step (to);
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
				ent.dir = act.to;
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
	public class ActAttack : Act {
		public Schema.Attack.A atk;
		public WUID dst;
		public ActAttack (Schema.Attack.A atk, WUID dst) {
			this.atk = atk;
			this.dst = dst;
		}
		public override bool Can (Entity ent) {
			Entity e = ent.world.FindEntity (dst);
			if (e == null)
				return false;
			return atk.s.i.Can (ent, e);
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
			}
			int Act.Step.Time (Entity ent) {
				return 3;
			}
		}
		private class Step2 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActAttack act = (ActAttack) Act.EntAct (ent);
				Entity dst = ent.world.FindEntity (act.dst);
				//TODO
				if (dst == null)
					return;
				if (ent.c.Manhattan (dst.c) > 1)
					return;
				act.atk.s.i.Damage (ent, dst);
			}
			int Act.Step.Time (Entity ent) {
				return 3;
			}
		}
		private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}

	[Serializable]
	public class ActIact : Act {
		public Schema.Iact.A iact;
		public WUID dst;
		public ActIact (Schema.Iact.A iact, WUID dst) {
			this.iact = iact;
			this.dst = dst;
		}
		public override bool Can (Entity ent) {
			Entity e = ent.world.FindEntity (dst);
			if (e == null)
				return false;
			return iact.s.i.Can (ent, e);
		}
		private class Step1 : Act.Step {
			void Act.Step.Do (Entity ent) {
			}
			int Act.Step.Time (Entity ent) {
				return 5; //TODO
			}
		}
		private class Step2 : Act.Step {
			void Act.Step.Do (Entity ent) {
				ActIact act = (ActIact) Act.EntAct (ent);
				Entity dst = ent.world.FindEntity (act.dst);
				if (dst == null)
					return;
				if (ent.c.Manhattan (dst.c) > 1)
					return;
				act.iact.s.i.Interact (ent, dst);
			}
			int Act.Step.Time (Entity ent) {
				return 0;
			}
		}
		private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
		public override Act.Step GetStep (int i) {
			return GetStep (i, steps);
		}
	}
}