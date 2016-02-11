//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Tree {
	[Serializable]
	public class Stat : Play.Stat<Stat.ID> {
		public enum ID {
			Branch,
			Fruit,
		}

		public Stat() : base() {
		}
		public Stat(Stat b) : base(b) {
		}

		public int branch {
			get { return ints[ID.Branch]; }
			set { ints[ID.Branch] = value; }
		}
		public int fruit {
			get { return ints[ID.Fruit]; }
			set { ints[ID.Fruit] = value; }
		}
	}
}
