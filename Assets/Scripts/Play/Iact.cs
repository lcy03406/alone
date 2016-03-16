//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	public static class Iact {
		public static string Display(this Schema.Iact iact) {
			return iact.ef.Display();
		}
		public static string Display(this Schema.Iact.A a) {
			return a.s.Display();
		}
		public static bool Can(this Schema.Iact.A a, Ctx ctx) {
			if (a.s.has_dst) {
				if (ctx.dst == null)
					return false;
				Schema.EntityStage dst_es = ctx.dst.GetAttr<Attrs.Core>().GetStage();
				if (dst_es == null || !dst_es.HasIactDst(a))
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
			Schema.EntityStage src_es = ctx.src.GetAttr<Attrs.Core>().GetStage();
			if (src_es == null || !src_es.HasIactSrc(a))
				return false;
			return a.s.ef.Can(ctx);
		}
		public static void Do(this Schema.Iact.A a, Ctx ctx, List<string> logs) {
			if (a.Can(ctx)) {
				if (logs != null) {
					logs.Add(GameLog(a, ctx));
				}
				a.s.ef.Do(ctx, logs);
				ctx.Do();
			}
		}
		public static string GameLog(this Schema.Iact.A a, Ctx ctx) {
			string log = ">>> ";
			if (ctx.src == null) {
				log += "layer " + ctx.layer.z;
			} else {
				log += ctx.src.GetName();
			}
			log += " ";
			log += a.s.name;
			if (ctx.dst != null) {
				log += " ";
				log += ctx.dst.GetName();
			}
			log += ".";
			return log;
		}
	}
}
