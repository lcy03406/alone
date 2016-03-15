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
			Part grow = ent.GetAttr<Part>();
			if (grow == null)
				return false;
			Part.PartItem part = grow.Get(id);
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
		bool must;

		public DecPart(Calc<Entity> ent, Schema.PartID id, Calc<int> value, bool must) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
			this.must = must;
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
			Part grow = ent.GetAttr<Part>();
			if (grow == null)
				return false;
			Part.PartItem part = grow.Get(id);
			if (part == null)
				return false;
			if (!c_value.Can(ctx))
				return false;
			if (must) {
				int value = c_value.Get(ctx);
				if (value > 0 && part.count < value)
					return false;
			}
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Part grow = ent.GetAttr<Part>();
			Part.PartItem part = grow.Get(id);
			int value = c_value.Get(ctx);
			if (value > 0) {
				int newvalue = part.count - value;
				if (newvalue < 0)
					newvalue = 0;
				grow.Set(id, newvalue);
			}
		}
	}

	public class DropPart : Effect {
		public readonly Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_value;

		public DropPart(Calc<Entity> ent, Schema.PartID id, Calc<int> value) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "drop " + id.ToString()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Part grow = ent.GetAttr<Part>();
			if (grow == null)
				return false;
			Part.PartItem part = grow.Get(id);
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
			Part grow = ent.GetAttr<Part>();
			Part.PartItem part = grow.Get(id);
			int value = c_value.Get(ctx);
			if (value > 0) {
				grow.Set(id, part.count - value);
			}
			Schema.Entity.A cre = Schema.Entity.GetA(Schema.EntityID.Item);
			Entity cent = cre.CreateEntity(ctx);
			Part.PartItem cpart = new Part.PartItem(part.a, value, 0, part.q, 0, 0);
			Part cgrow = cent.GetAttr<Part>();
			cgrow.AddPart(Schema.PartID.Item, cpart);
		}
	}

	public class DropAllPart : Effect {
		public readonly Calc<Entity> c_ent;

		public DropAllPart(Calc<Entity> ent) {
			c_ent = ent;
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "destruct.\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Part grow = ent.GetAttr<Part>();
			if (grow == null)
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Part grow = ent.GetAttr<Part>();
			foreach (Part.PartItem part in grow.items.Values) {
				Schema.Entity.A cre = Schema.Entity.GetA(Schema.EntityID.Item);
				Entity cent = cre.CreateEntity(ctx);
				Part.PartItem cpart = new Part.PartItem(part.a, part.count, 0, part.q, 0, 0);
				Part cgrow = cent.GetAttr<Part>();
				cgrow.AddPart(Schema.PartID.Item, cpart);
				part.count = 0;
			}
		}
	}
}
