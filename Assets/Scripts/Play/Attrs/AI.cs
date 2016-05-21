//utf-8。
using System;
using System.Collections.Generic;
using Play.Acts;

namespace Play.Attrs {
	[Serializable]
	public abstract class AI : Attrib {
		public abstract void NextAct();
		Queue<Act> next = new Queue<Act>();

		public Act Deque() {
			if (next.Count == 0)
				NextAct();
			if (next.Count == 0)
				return null;
			return next.Dequeue();
		}

		public void Enque(Act act) {
			next.Enqueue(act);
		}
	}

	[Serializable]
	public class AIHuman : AI {
		static Random random = new Random ();
		public override void NextAct() {
			Direction r = (Direction)random.Next (9);
			if (r == Direction.None || r == Direction.Center) {
				Enque(new Acts.ActIact(Schema.Iact.GetA(Schema.ActionID.Rest), null));
			} else {
				Pos pos = ent.GetAttr<Pos>();
                if (r != pos.dir)
					Enque(new ActIact(Schema.Iact.GetA(Schema.ActionID.Dir), pos.c.Step(r)));
				Enque(new ActIact(Schema.Iact.GetA(Schema.ActionID.Move), null));
			}
		}
	}
}
