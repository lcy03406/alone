//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play.Eff {
	public class UsePart : Effect {
		public readonly Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_min;
		Calc<int> c_max;

		public UsePart(Calc<Entity> ent, Schema.PartID id, Calc<int> min, Calc<int> max) {
			c_ent = ent;
			this.id = id;
			this.c_min = min;
			this.c_max = max;
		}

		public override string Display() {
			string disp = c_ent.Display() + ": "
				+ id.ToString() + " must be";
			if (c_min != null) {
				disp += " at least " + c_min.Display();
			}
			if (c_max != null) {
				disp += " at most " + c_max.Display();
			}
			return disp + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Grow grow = ent.GetAttr<Grow>();
			if (grow == null)
				return false;
			Grow.Part part = grow.Get(id);
			if (part == null)
				return false;
			int value = part.count;
			if (c_min != null) {
				if (!c_min.Can(ctx))
					return false;
				int min = c_min.Get(ctx);
				if (value < min)
					return false;
			}
			if (c_max != null) {
				if (!c_max.Can(ctx))
					return false;
				int max = c_max.Get(ctx);
				if (value > max)
					return false;
			}
			return true;
		}

		public override void Do(Ctx ctx) {
		}
	}

	public class DecPart : Effect {
		public readonly Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_value;

		public DecPart(Calc<Entity> ent, Schema.PartID id, Calc<int> value) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "consume " + id.ToString()
				+ " by " + c_value.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Grow grow = ent.GetAttr<Grow>();
			if (grow == null)
				return false;
			Grow.Part part = grow.Get(id);
			if (part == null)
				return false;
			if (!c_value.Can(ctx))
				return false;
			int value = c_value.Get(ctx);
			if (value > 0 && part.count < value)
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Grow grow = ent.GetAttr<Grow>();
			Grow.Part part = grow.Get(id);
			int value = c_value.Get(ctx);
			if (value > 0) {
				grow.Set(id, part.count - value);
			}
		}
	}
}
