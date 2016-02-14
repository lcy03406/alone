//utf-8。
using System;

namespace Play {
	public class Iact {
		public int time1;
		public int time2;
		public Effect ef;
		public Iact(int time1, int time2, Effect[] eff) {
			this.time1 = time1;
			this.time2 = time2;
			this.ef = new EffMulti(eff);
		}
		public bool Can (Entity src, Entity dst) {
			Ctx ctx = new Ctx {
				src = src,
				dst = dst,
			};
			if (src.c.Manhattan(dst.c) > 1)
				return false;
			return ef.Can(ctx);
		}
		public void Interact (Entity src, Entity dst) {
			Ctx ctx = new Ctx {
				src = src,
				dst = dst,
			};
			ef.Do(ctx);
		}
	}
}
