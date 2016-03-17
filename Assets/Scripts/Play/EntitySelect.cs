//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class EntitySelect {
		public int blockade;
		public int iact_dst;
		public bool Select(Entity ent) {
			Attrs.Core core = ent.GetAttr<Attrs.Core>();
			if (core == null)
				return false;
			if (blockade != 0) {
				bool got = core.GetBlockade();
				bool want = (blockade > 0);
				if (got != want)
					return false;
			}
			if (iact_dst != 0) {
				Schema.EntityStage es = core.GetStage();
				bool got = (es != null && es.iact_dst != null);
				bool want = (iact_dst > 0);
				if (got != want)
					return false;
			}
			return true;
		}
	}
}
