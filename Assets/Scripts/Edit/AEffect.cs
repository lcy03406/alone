//utf-8。
using System;
using System.Collections.Generic;
using Ctx = Play.Ctx;
using Entity = Play.Entity;

namespace Edit {
	public sealed class AEffect : Meta {
		public bool dst;
		public Play.Effect ef;
		public List<int> param;

		public override void AfterLoad() {
			ef.AfterLoad(dst, param);
		}
	}

	public abstract class EffectFunc {
		public abstract void AfterLoad(bool dst, List<int> param);
		public abstract string Display();
		public abstract bool Can(Ctx ctx);
		public abstract void Do(Ctx ctx, List<string> logs);
	}
}
