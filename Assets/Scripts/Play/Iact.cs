//utf-8。
using System.Collections.Generic;

namespace Play {
	public class Iact {
		Schema.Iact.A a;
		public Iact(Schema.Iact.A a) {
			this.a = a;
		}
		public override string ToString() {
			return Display();
		}
		public string Display() {
			return a.s.ef.Display();
		}
		public bool Can (Ctx ctx) {
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
		public void Do (Ctx ctx) {
			if (Can(ctx)) {
				a.s.ef.Do(ctx);
				ctx.Do();
			}
		}
	}
}
