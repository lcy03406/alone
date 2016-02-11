//utf-8。
using System;

namespace Play {
	[Serializable]
	public class Iact {
		public Effect[] eff;
		public bool Can (Entity src, Entity dst) {
			Ctx ctx = new Ctx {
				src = src,
				dst = dst,
			};
			foreach (Effect ef in eff) {
				if (!ef.Can(ctx))
					return false;
			}
			return true;
		}
		public void Interact (Entity src, Entity dst) {
			Ctx ctx = new Ctx {
				src = src,
				dst = dst,
			};
			foreach (Effect ef in eff) {
				ef.Do(ctx);
			}
		}
	}
}
