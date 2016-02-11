//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Creature {
	[Serializable]
	public class Stat : Play.Stat<Stat.ID> {
		public enum ID {
			HitPoint,
			Stamina,
			Damage,
		}
		public Stat() : base() {
		}
		public Stat(Stat b) : base(b) {
		}
		public int hp {
			get { return ints[ID.HitPoint]; }
			set { ints[ID.HitPoint] = value; }
		}
		public int sta {
			get { return ints[ID.Stamina]; }
			set { ints[ID.Stamina] = value; }
		}
	}
}
