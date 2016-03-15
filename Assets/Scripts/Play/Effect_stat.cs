//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

using StatID = Schema.StatID;

namespace Play.Eff {
	public class UseStat : Effect {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_min;
		Calc<int> c_max;

		public UseStat(Calc<Entity> ent, StatID id, Calc<int> min, Calc<int> max) {
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
			Stat stat = ent.GetAttr<Stat>();
			if (stat == null)
				return false;
			int value = stat.Get(id);
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

	public class IncStat : Effect {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_value;

		public IncStat(Calc<Entity> ent, StatID id, Calc<int> value) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "increase " + id.ToString()
				+ " by " + c_value.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat stat = ent.GetAttr<Stat>();
			if (stat == null)
				return false;
			if (!c_value.Can(ctx))
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			int value = c_value.Get(ctx);
			stat.Set(id, stat.Get(id) + value);
		}
	}

	public class DecStat : Effect {
		public readonly Calc<Entity> c_ent;
		StatID id;
		Calc<int> c_value;
		bool must;

		public DecStat(Calc<Entity> ent, StatID id, Calc<int> value, bool must) {
			c_ent = ent;
			this.id = id;
			this.c_value = value;
			this.must = must;
		}

		public override string Display() {
			return c_ent.Display() + ": "
				+ "decrease " + id.ToString()
				+ " by " + c_value.Display()
				+ ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat stat = ent.GetAttr<Stat>();
			if (stat == null)
				return false;
			if (!c_value.Can(ctx))
				return false;
			if (must) {
				int value = c_value.Get(ctx);
				if (value > 0 && stat.Get(id) < value)
					return false;
			}
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			int value = c_value.Get(ctx);
            if (value > 0) {
				int oldvalue = stat.Get(id);
				int newvalue = oldvalue - value;
				if (newvalue < 0)
					newvalue = 0;
				stat.Set(id, newvalue);
			}
		}
	}
}
