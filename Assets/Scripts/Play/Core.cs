//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public abstract class Core : Attrib {
		public abstract List<Schema.Iact.A> ListIact (Entity src);
	}
}