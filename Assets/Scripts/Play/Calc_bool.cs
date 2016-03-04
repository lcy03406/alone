//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play.Calcs {
	public class And : Calc<bool> {
		public readonly Calc<bool>[] list;

		public override string Display() {
			string disp = "all of {";
			foreach (Calc<bool> calc in list) {
				disp += calc.Display();
				disp += ", ";
			}
			return disp + "}";
		}

		public override bool Can(Ctx ctx) {
			foreach (Calc<bool> calc in list) {
				if (!calc.Can(ctx))
					return false;
				if (!calc.Get(ctx))
					return true;
			}
			return true;
		}

		public override bool Get(Ctx ctx) {
			foreach (Calc<bool> calc in list) {
				if (!calc.Can(ctx))
					return false;
				if (!calc.Get(ctx))
					return false;
			}
			return true;
		}
	}

	public class HasStat : Calc<bool> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.StatID id;
		public HasStat(Calc<Entity> ent, Schema.StatID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public override string Display() {
			return id.ToString() + " of " + c_ent.Display();
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

		public override bool Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			return stat.Get(id) > 0;
		}
	}

	public class HasPart : Calc<bool> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.PartID id;
		public HasPart(Calc<Entity> ent, Schema.PartID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public override string Display() {
			return id.ToString() + " of " + c_ent.Display();
		}
		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Attrs.Part grow = ent.GetAttr<Attrs.Part>();
			if (grow == null)
				return false;
			Attrs.Part.PartItem part = grow.Get(this.id);
			if (part == null)
				return false;
			return true;
		}

		public override bool Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Attrs.Part grow = ent.GetAttr<Attrs.Part>();
			Attrs.Part.PartItem part = grow.Get(this.id);
			return part.count > 0;
		}
	}
}
