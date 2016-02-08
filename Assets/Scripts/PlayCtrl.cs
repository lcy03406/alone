using System;
using System.Collections.Generic;

[Serializable]
public class PlayCtrl : PlayAI{
	
	Queue<EntityAct> next = new Queue<EntityAct>();

	public override EntityAct NextAct () {
		if (next.Count <= 0)
			return null;
		return next.Dequeue ();
	}

	public bool CmdMove (Direction to) {
		if (to == Direction.None || to == Direction.Center) {
			return false;
		}
		EntityAct act;
		if (to == ent.d.dir) {
			act = new PlayActMove (to);
		} else {
			act = new PlayActDir (to);
		}
		next.Enqueue (act);
		return true;
	}
	
	public bool CmdAttack () {
		if (ent.d.dir == Direction.None || ent.d.dir == Direction.Center) {
			return false;
		}
		EntityAct act = new PlayActAttack ();
		next.Enqueue (act);
		return true;
	}

	public bool CmdInteract(IactDst to) {
		EntityAct act = new PlayActIact (to);
		next.Enqueue (act);
		return true;
	}

	public 
	
}

