//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play.Calcs {
	public class Damage : Calc<int> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.StatID id;
		public readonly int add;
		public readonly int mul;
		public Damage(Calc<Entity> ent, Schema.StatID id, int mul, int add) {
			this.c_ent = ent;
			this.id = id;
			this.add = add;
			this.mul = mul;
		}

		public override string Display() {
			if (mul == 0)
				return add.ToString();
			if (add == 0)
				return string.Format("{0}% {1}", mul, id);
			else
				return string.Format("{0}% {1} + {2}", mul, id, add);
		}
		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat stat = ent.GetAttr<Stat>();
			if (stat == null)
				return false;
			return true;
		}

		public override int Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			int st = stat.Get(id);
			return st * mul / 100 + add;
		}
	}
}
