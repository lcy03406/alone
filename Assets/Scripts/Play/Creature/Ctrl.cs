//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Creature {
	[Serializable]
	public class Ctrl : AI {

		Queue<Act> next = new Queue<Act> ();

		public override Act NextAct () {
			if (next.Count <= 0)
				return null;
			return next.Dequeue ();
		}

		public bool CmdMove (Direction to) {
			if (to == Direction.None || to == Direction.Center) {
				return false;
			}
			Act act;
			if (to == ent.dir) {
				act = new ActMove (to);
			} else {
				act = new ActDir (to);
			}
			next.Enqueue (act);
			return true;
		}

		public bool CmdAttack () {
			if (ent.dir == Direction.None || ent.dir == Direction.Center) {
				return false;
			}
			//TODO
			Schema.Attack.A atk = Schema.Attack.GetA (Schema.Attack.ID.Punch);
			Entity dst = ent.world.SearchEntity (ent.c.Step (ent.dir));
			if (dst == null)
				return false;
			Act act = new ActAttack (atk, dst.id);
			next.Enqueue (act);
			return true;
		}

		public bool CmdIact (Schema.Iact.A iact, WUID dst) {
			Act act = new ActIact (iact, dst);
			next.Enqueue (act);
			return true;
		}

		public Entity ListDst () {
			Entity dst = ent.world.SearchEntity (ent.c.Step (ent.dir));
			return dst;
		}

		public List<Schema.Iact.A> ListIact (Entity dst) {
			Play.Core core = dst.GetAttr<Play.Core> ();
			if (core == null)
				return null;
			return core.ListIact (ent);
		}
	}
}

