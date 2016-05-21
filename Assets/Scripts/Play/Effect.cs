//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;
using Edit;

namespace Play {
	public abstract class Effect {
		public abstract void AfterLoad(bool dst, List<int> param);
		public abstract string Display();
		public abstract bool Can(Ctx ctx);
		public abstract void Do(Ctx ctx, List<string> logs);

		public override string ToString() {
			return Display();
		}
	}
}

namespace Play.Eff {
	public class Multi : Effect {
		public List<AEffect> eff;

		public override void AfterLoad(bool dst, List<int> param) {
			foreach (int id in param) {
				eff.Add(All.all.Get<AEffect>(id));
			}
		}
		public override string Display() {
			string disp = "all of: \n";
			foreach (AEffect ef in eff) {
				disp += ef.ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (AEffect ef in eff) {
				if (!ef.ef.Can(ctx))
					return false;
			}
			return true;
		}

		public override void Do(Ctx ctx, List<string> logs) {
			foreach (AEffect ef in eff) {
				ef.ef.Do(ctx, logs);
			}
		}
	}

	public class Any : Effect {
		public List<AEffect> eff;

		public override void AfterLoad(bool dst, List<int> param) {
			foreach (int id in param) {
				eff.Add(All.all.Get<AEffect>(id));
			}
		}
		public override string Display() {
			string disp = "any of: \n";
			foreach (AEffect ef in eff) {
				disp += ef.ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (AEffect ef in eff) {
				if (ef.ef.Can(ctx)) {
					return true;
				}
			}
			return false;
		}

		public override void Do(Ctx ctx, List<string> logs) {
			foreach (AEffect ef in eff) {
				if (ef.ef.Can(ctx))
					ef.ef.Do(ctx, logs);
			}
		}
	}

	public class One : Effect {
		public List<AEffect> eff;

		public override void AfterLoad(bool dst, List<int> param) {
			foreach (int id in param) {
				eff.Add(All.all.Get<AEffect>(id));
			}
		}
		public override string Display() {
			string disp = "one of: \n";
			foreach (AEffect ef in eff) {
				disp += ef.ef.Display();
			}
			return disp;
		}
		public override bool Can(Ctx ctx) {
			foreach (AEffect ef in eff) {
				if (ef.ef.Can(ctx)) {
					return true;
				}
			}
			return false;
		}

		public override void Do(Ctx ctx, List<string> logs) {
			foreach (AEffect ef in eff) {
				if (ef.ef.Can(ctx)) {
					ef.ef.Do(ctx, logs);
				}
			}
		}
	}
}
