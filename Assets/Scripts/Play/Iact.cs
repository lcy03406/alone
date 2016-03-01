//utf-8。
using System.Collections.Generic;

namespace Play {
	public static class Iact {
		public static string Display(this Schema.Iact iact) {
			return iact.ef.Display();
		}
		public static string Display(this Schema.Iact.A a) {
			return a.s.Display();
		}
		public static bool Can (this Schema.Iact.A a, Ctx ctx) {
			if (a.s.has_dst) {
				if (ctx.dst == null)
					return false;
				if (a.s.distance >= 0) {
					Attrs.Pos sp = ctx.src.GetAttr<Attrs.Pos>();
					Attrs.Pos dp = ctx.dst.GetAttr<Attrs.Pos>();
					if (sp == null || dp == null)
						return false;
					if (sp.c.Manhattan(dp.c) > a.s.distance)
						return false;
				}
			}
			return a.s.ef.Can(ctx);
		}
		public static void Do (this Schema.Iact.A a, Ctx ctx) {
			if (a.Can(ctx)) {
				a.s.ef.Do(ctx);
				ctx.Do();
			}
		}
	}
}
