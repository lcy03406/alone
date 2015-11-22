using System;
using System.Collections.Generic;

[Serializable]
public abstract class PlayAI {
	[NonSerialized]
	public WorldEntity ent;

	public abstract EntityAct NextAct ();
}

[Serializable]
public class PlayAIHuman : PlayAI{
	static Random random = new Random();
	public override EntityAct NextAct () {
		Direction r = (Direction)random.Next (9);
		if (r == Direction.None || r == Direction.Center)
			return new PlayActWait ();
		return new PlayActMove (r);
		//TODO
	}
}
