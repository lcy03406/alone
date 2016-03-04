//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.PartID;

namespace Play.Attrs {
	[Serializable]
	public class Buff : Attrib {
		[Serializable]
		public class Buf {
			public Schema.StatID stat;
			public int value;
		}
	}
}
