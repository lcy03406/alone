using System;
using System.Collections.Generic;

[Serializable]
public abstract class PlayAI {
	[NonSerialized]
	public WorldEntity ent;

	public abstract PlayAct NextAct ();
}

[Serializable]
public class PlayAIHuman : PlayAI{
	static Random random = new Random();
	public override PlayAct NextAct () {
		int r = random.Next (9);
		return new PlayActMove ((Direction)r);
		//TODO
	}
}
