//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;
using Edit;

using StatID = Edit.AStat;

namespace Play.Eff {
	public class UseStat : Effect {
		public Calc<Entity> c_ent;
		StatID id;
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
			id = All.all.Get<AStat>(param[0]);
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

		public override void Do(Ctx ctx, List<string> logs) {
		}
	}

	public class IncStat : Effect {
		public Calc<Entity> c_ent;
		StatID id;
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
			id = All.all.Get<AStat>(param[0]);
			c_value = new Calcs.Const<int>(param[1]);
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			int value = c_value.Get(ctx);
			stat.Set(id, stat.Get(id) + value);
			if (logs != null) {
				int newvalue = stat.Get(id);
				logs.Add(string.Format("{0}'s {1} increase to {2}.", ent.GetName(), id, newvalue));
			}
		}
	}

	public class DecStat : Effect {
		public Calc<Entity> c_ent;
		StatID id;
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
			id = All.all.Get<AStat>(param[0]);
			c_value = new Calcs.Const<int>(param[1]);
			must = param[2] > 0;
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			int value = c_value.Get(ctx);
            if (value > 0) {
				int oldvalue = stat.Get(id);
				int newvalue = oldvalue - value;
				if (newvalue < 0)
					newvalue = 0;
				stat.Set(id, newvalue);
				if (logs != null) {
					logs.Add(string.Format("{0}'s {1} decrease to {2}.", ent.GetName(), id, newvalue));
				}
			}
		}
	}
}
