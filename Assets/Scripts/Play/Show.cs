//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public abstract class Show : Attrib {
		public abstract Schema.Sprite.A Sprite ();
	}
}