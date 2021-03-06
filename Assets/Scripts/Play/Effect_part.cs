//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play.Eff {
	public class UsePart : Effect {
		public Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_min;
		Calc<int> c_max;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 3) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			id = (Schema.PartID)param[0];
			c_min = new Calcs.Const<int>(param[1]);
			c_max = new Calcs.Const<int>(param[2]);
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

		public override void Do(Ctx ctx, List<string> logs) {
		}
	}

	public class DecPart : Effect {
		public Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_value;
		bool must;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 3) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			id = (Schema.PartID)param[0];
			c_value = new Calcs.Const<int>(param[1]);
			must = param[2] > 0;
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Part grow = ent.GetAttr<Part>();
			Part.PartItem part = grow.Get(id);
			int value = c_value.Get(ctx);
			if (value > 0) {
				int newvalue = part.count - value;
				if (newvalue < 0)
					newvalue = 0;
				grow.Set(id, newvalue);
				if (logs != null) {
					logs.Add(string.Format("{0}'s {1} decrease to {2}.", ent.GetName(), id, newvalue));
				}
			}
		}
	}

	public class DropPart : Effect {
		public Calc<Entity> c_ent;
		Schema.PartID id;
		Calc<int> c_value;

		public override void AfterLoad(bool dst, List<int> param) {
			if (param.Count < 2) {
				throw new GameResourceException(string.Format("param count {0}", param.Count));
			}
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
			//TODO
			id = (Schema.PartID)param[0];
			c_value = new Calcs.Const<int>(param[1]);
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Part grow = ent.GetAttr<Part>();
			Part.PartItem part = grow.Get(id);
			int value = c_value.Get(ctx);
			if (value > 0) {
				int newvalue = part.count - value;
				if (newvalue < 0)
					newvalue = 0;
				value = newvalue - part.count;
				grow.Set(id, newvalue);
				if (logs != null) {
					logs.Add(string.Format("{0}'s {1} decrease to {2}.", ent.GetName(), id, newvalue));
				}
			}
			if (value > 0) {
				Schema.Entity.A cre = Schema.Entity.GetA(Schema.EntityID.Item);
				Part.PartItem cpart = new Part.PartItem(part.a, value, value, part.q, 0, 0);
				cre.CreateEntity(ctx, false, cpart);
			}
		}
	}

	public class DropAllPart : Effect {
		public Calc<Entity> c_ent;

		public override void AfterLoad(bool dst, List<int> param) {
			if (dst) {
				c_ent = new Calcs.Dst();
			} else {
				c_ent = new Calcs.Src();
			}
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Part grow = ent.GetAttr<Part>();
			foreach (Part.PartItem part in grow.items.Values) {
				if (part.count > 0) {
					Schema.Entity.A cre = Schema.Entity.GetA(Schema.EntityID.Item);
					Part.PartItem cpart = new Part.PartItem(part.a, part.count, part.count, part.q, 0, 0);
					cre.CreateEntity(ctx, false, cpart);
					part.count = 0;
				}
			}
			if (logs != null) {
				logs.Add(ent.GetName() + " collapse.");
			}
		}
	}
}
