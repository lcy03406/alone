using System;
using System.Collections.Generic;

[Serializable]
public abstract class PlayAct {
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
public class PlayActMove : PlayAct {
	public Coord to;
	public PlayActMove (Coord to) {
		this.to = to;
	}
	public override bool Can (WorldEntity ent) {
		WorldEntity e = ent.world.SearchEntity (to);
		if (e != null)
			return false;
		return true;
	}
	private class Step1 : PlayAct.Step {
		void PlayAct.Step.Do (WorldEntity ent) {
		}
		int PlayAct.Step.Time (WorldEntity ent) {
			return 5;
		}
	}
	private class Step2 : PlayAct.Step {
		void PlayAct.Step.Do (WorldEntity ent) {
			PlayActMove act = (PlayActMove) ent.d.act;
			Coord to = act.to;
			World world = ent.world;
			world.MoveEntity (ent, to);
		}
		int PlayAct.Step.Time (WorldEntity ent) {
			return 5;
		}
	}
	private static Step[] steps = new Step[] {new Step1(), new Step2() };
	public override PlayAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActDir : PlayAct {
	public Direction to;
	public PlayActDir (Direction to) {
		this.to = to;
	}
	public override bool Can (WorldEntity ent) {
		return true;
	}
	private class Step1 : PlayAct.Step {
		void PlayAct.Step.Do (WorldEntity ent) {
			PlayActDir act = (PlayActDir) ent.d.act;
			ent.d.dir = act.to;
		}
		int PlayAct.Step.Time (WorldEntity ent) {
			return 0;
		}
	}
	private static Step[] steps = new Step[] {new Step1()};
	public override PlayAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}

[Serializable]
public class PlayActAttack : PlayAct {
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
	private class Step1 : PlayAct.Step {
		void PlayAct.Step.Do (WorldEntity ent) {
		}
		int PlayAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private class Step2 : PlayAct.Step {
		void PlayAct.Step.Do (WorldEntity ent) {
			PlayActAttack act = (PlayActAttack) ent.d.act;
			WorldEntity target = ent.world.FindEntity(act.target);
			//TODO
			if (target != null)
				target.BeAttack();
		}
		int PlayAct.Step.Time (WorldEntity ent) {
			return 3;
		}
	}
	private static Step[] steps = new Step[] {new Step1(), new Step2() };
	public override PlayAct.Step GetStep (int i) {
		return GetStep (i, steps);
	}
}