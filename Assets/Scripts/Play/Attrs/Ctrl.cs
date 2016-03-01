//utf-8。
using System;
using System.Collections.Generic;
using Play.Acts;

namespace Play.Attrs {
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
			Pos pos = ent.GetAttr<Pos>();
			Act act;
			if (to == pos.dir) {
				act = new ActMove (to);
			} else {
				act = new ActDir (to);
			}
			next.Enqueue (act);
			return true;
		}

		public bool CmdAttack () {
			Pos pos = ent.GetAttr<Pos>();
			if (pos.dir == Direction.None || pos.dir == Direction.Center) {
				return false;
			}
			//TODO
			Schema.Iact.A atk = Schema.Iact.GetA (Schema.Iact.ID.Attack_Punch);
			Entity dst = ent.world.SearchEntity (pos.c.Step (pos.dir));
			if (dst == null)
				return false;
			Act act = new ActIact (atk, dst.id);
			next.Enqueue (act);
			return true;
		}

		public bool CmdIact (Schema.Iact.A iact, WUID dst) {
			Act act = new ActIact (iact, dst);
			next.Enqueue (act);
			return true;
		}

		public Entity ListDst () {
			Pos pos = ent.GetAttr<Pos>();
			Entity dst = ent.world.SearchEntity (pos.c.Step (pos.dir));
			return dst;
		}

		public List<Schema.Iact.A> ListIact (Entity dst) {
			Core core = dst.GetAttr<Core> ();
			if (core == null)
				return null;
			return core.ListIact (ent);
		}

		public List<Schema.Iact.A> ListMake() {
			Core core = ent.GetAttr<Core>();
			if (core == null)
				return null;
			return core.ListMake();
		}
	}
}
