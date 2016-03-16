//utf-8。
using System;
using System.Collections.Generic;
using Play.Acts;

namespace Play.Attrs {
	[Serializable]
	public class Ctrl : AI {

		public override void NextAct() {
		}

		public bool CmdMove (Direction to) {
			if (to == Direction.None || to == Direction.Center) {
				return false;
			}
			Pos pos = ent.GetAttr<Pos>();
			Coord toc = pos.c.Step(to);
			if (to != pos.dir) {
				Enque(new ActIact(Schema.Iact.GetA(Schema.ActionID.Dir), toc));
			}
			Entity dst = ent.layer.SearchEntity(toc);
			if (dst == null) {
				Enque(new ActIact(Schema.Iact.GetA(Schema.ActionID.Move), WUID.None));
			} else {
				Core tocore = dst.GetAttr<Core>();
				Schema.Iact.A iact = tocore.GetIactAuto(ent);
				if (iact != null) {
					Enque(new ActIact(iact, dst.id));
				}
			}
			return true;
		}

		public bool CmdAttack () {
			Pos pos = ent.GetAttr<Pos>();
			if (pos.dir == Direction.None || pos.dir == Direction.Center) {
				return false;
			}
			//TODO
			Schema.Iact.A atk = Schema.Iact.GetA (Schema.ActionID.AttackPunch);
			Entity dst = ent.layer.SearchEntity (pos.c.Step (pos.dir));
			if (dst == null)
				return false;
			Act act = new ActIact (atk, dst.id);
			Enque(act);
			return true;
		}

		public bool CmdIact (Schema.Iact.A iact, WUID dst) {
			Act act = new ActIact (iact, dst);
			Enque(act);
			return true;
		}

		public Entity ListDst () {
			Pos pos = ent.GetAttr<Pos>();
			Entity dst = ent.layer.SearchEntity (pos.c.Step (pos.dir));
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

