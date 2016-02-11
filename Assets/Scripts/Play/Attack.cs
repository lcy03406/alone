//utf-8。
using System;

namespace Play {
	public abstract class Attack {
		abstract public bool Can (Entity src, Entity dst);
		abstract public void Damage (Entity src, Entity dst);
	}
}
