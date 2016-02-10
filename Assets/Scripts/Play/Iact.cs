//utf-8。
using System;

namespace Play {
	[Serializable]
	public abstract class Iact {
		abstract public bool Can (Entity src, Entity dst);
		abstract public void Interact (Entity src, Entity dst);
	}
}
