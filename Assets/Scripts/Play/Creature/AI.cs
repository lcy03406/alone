//utf-8。
using System;

namespace Play.Creature {
	[Serializable]
	public abstract class AI : Attrib {
		public abstract Act NextAct ();
	}

	[Serializable]
	public class AIHuman : AI {
		static Random random = new Random ();
		public override Act NextAct () {
			Direction r = (Direction)random.Next (9);
			if (r == Direction.None || r == Direction.Center)
				return new ActWait ();
			return new ActMove (r);
			//TODO
		}
	}
}
