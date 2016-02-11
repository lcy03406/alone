//utf-8。
using System;
using System.Collections.Generic;
using Schema;

namespace Play.Tree {
	public class CalcPartItem : Calc<Schema.Item.A> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.Tree.Part part;
		public CalcPartItem(Calc<Entity> ent, Schema.Tree.Part part) {
			this.c_ent = ent;
			this.part = part;
		}

		public bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Core core = ent.GetAttr<Core>();
			if (core == null)
				return false;
			return true;
		}

		public Schema.Item.A Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Core core = ent.GetAttr<Core>();
			return core.race.s.items[part];
		}
	}
}
