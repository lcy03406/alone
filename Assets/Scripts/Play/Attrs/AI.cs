//utf-8。
using System;

namespace Play.Attrs {
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
				return new Acts.ActIact(Schema.Iact.GetA(Schema.ActionID.Rest), WUID.None);
			return new Acts.ActMove (r);
			//TODO
		}
	}
}
