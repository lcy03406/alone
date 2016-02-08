using System;
using System.Collections.Generic;

[Serializable]
public abstract class EntityAct {
	public interface Step {
		void Do (WorldEntity ent);
		int Time (WorldEntity ent);
	}
	public abstract bool Can (WorldEntity ent);
	public bool Load (WorldEntity ent) { return true; }
	public abstract Step GetStep (int i);
	static protected Step GetStep (int i, Step[] steps) {
		if (i >= 0 && i < steps.Length)
			return steps[i];
		return null;
	}
}

[Serializable]
public class PlayActWait : EntityAct {
	public PlayActWait () {
	}
	public override bool Can (WorldEntity ent) {
		return true;
	}
	private class Step1 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 1;
		}
	}
	private static Step[] steps = new Step[] {new Step1()};
	public override EntityAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActMove : EntityAct {
	public Direction to;
	public PlayActMove (Direction to) {
		this.to = to;
	}
	public override bool Can (WorldEntity ent) {
		return ent.world.CanMoveTo (ent.d.c.Step (to));
	}
	private class Step1 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
			PlayActMove act = (PlayActMove) ent.d.act;
			Direction to = act.to;
			ent.d.dir = to;
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 5;
		}
	}
	private class Step2 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
			PlayActMove act = (PlayActMove) ent.d.act;
			Direction to = act.to;
			Coord tc = ent.d.c.Step (to);
			World world = ent.world;
			if (world.CanMoveTo (tc)) {
				world.MoveEntity (ent, tc);
			}
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 5;
		}
	}
	private static Step[] steps = new Step[] {new Step1(), new Step2() };
	public override EntityAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActDir : EntityAct {
	public Direction to;
	public PlayActDir (Direction to) {
		this.to = to;
	}
	public override bool Can (WorldEntity ent) {
		return true;
	}
	private class Step1 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
			PlayActDir act = (PlayActDir) ent.d.act;
			ent.d.dir = act.to;
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 0;
		}
	}
	private static Step[] steps = new Step[] {new Step1()};
	public override EntityAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActAttack : EntityAct {
	public WUID target;
	public PlayActAttack () {
	}
	public override bool Can (WorldEntity ent) {
		Coord to = ent.d.c.Step (ent.d.dir);
		WorldEntity e = ent.world.SearchEntity (to);
		if (e == null)
			return false;
		target = e.d.id;
		return true;
	}
	private class Step1 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private class Step2 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
			PlayActAttack act = (PlayActAttack) ent.d.act;
			WorldEntity target = ent.world.FindEntity(act.target);
			//TODO
			if (target != null)
				target.BeAttack();
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private static Step[] steps = new Step[] {new Step1(), new Step2() };
	public override EntityAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActIact : EntityAct {
	public IactDst iad;
	public PlayActIact (IactDst iad) {
		this.iad = iad;
	}
	public override bool Can (WorldEntity ent) {
		WorldEntity e = ent.world.FindEntity (iad.dst);
		if (e == null)
			return false;
		return e.d.core.CanIact (ent, iad.ia);
	}
	private class Step1 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private class Step2 : EntityAct.Step {
		void EntityAct.Step.Do (WorldEntity ent) {
			PlayActAttack act = (PlayActAttack)ent.d.act;
			WorldEntity target = ent.world.FindEntity (act.target);
			//TODO
			if (target != null)
				target.BeAttack ();
		}
		int EntityAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private static Step[] steps = new Step[] { new Step1 (), new Step2 () };
	public override EntityAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}