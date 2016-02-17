//utf-8。
using System;

namespace Play {
	[Serializable]
	public abstract class Attrib {
		[NonSerialized]
		public Entity ent;
		public virtual void SetEntity (Entity ent) {
			this.ent = ent;
		}
		public virtual int NextTick () {
			return 0;
		}
		public virtual void Tick (int time) {
		}
	}
}