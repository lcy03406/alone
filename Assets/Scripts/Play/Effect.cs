//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play {
	public abstract class Effect {
		public abstract string Display();
		public abstract bool Can(Ctx ctx);
		public abstract void Do(Ctx ctx);

		public override string ToString() {
			return Display();
		}
	}
}

namespace Play.Eff {
	public class Multi : Effect {
		public Effect[] eff;
		public Multi(Effect[] eff) {
			this.eff = eff;
		}
		public override string Display() {
			string disp = "all of: \n";
			foreach (Effect ef in eff) {
				disp += ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (!ef.Can(ctx))
					return false;
			}
			return true;
		}

		public override void Do(Ctx ctx) {
			foreach (Effect ef in eff) {
				ef.Do(ctx);
			}
		}
	}

	public class Any : Effect {
		public Effect[] eff;
		public Any(Effect[] eff) {
			this.eff = eff;
		}
		public override string Display() {
			string disp = "any of: \n";
			foreach (Effect ef in eff) {
				disp += ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (ef.Can(ctx)) {
					return true;
				}
			}
			return false;
		}

		public override void Do(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (ef.Can(ctx))
					ef.Do(ctx);
			}
		}
	}

	public class One : Effect {
		public Effect[] eff;
		public One(Effect[] eff) {
			this.eff = eff;
		}
		public override string Display() {
			string disp = "one of: \n";
			foreach (Effect ef in eff) {
				disp += ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (ef.Can(ctx)) {
					return true;
				}
			}
			return false;
		}

		public override void Do(Ctx ctx) {
			foreach (Effect ef in eff) {
				if (ef.Can(ctx)) {
					ef.Do(ctx);
					return;
				}
			}
		}
	}

	public class Must : Effect {
		public readonly Calc<bool> cond;
		public Must(Calc<bool> cond) {
			this.cond = cond;
		}

		public override string Display() {
			return "must: " + cond.Display() + ".\n";
		}

		public override bool Can(Ctx ctx) {
			return cond.Can(ctx);
		}

		public override void Do(Ctx ctx) {
		}
	}
}
