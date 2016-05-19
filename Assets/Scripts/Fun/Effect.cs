//utf-8。
using System;
using System.Collections.Generic;
using Edit;
using Play;
using Ctx = Play.Ctx;

namespace Fun {
	//TODO
	public abstract class EffectFunc {
		public virtual int param1 { get; set; }
		public virtual int param2 { get; set; }
		public virtual int param3 { get; set; }
		public virtual int param4 { get; set; }
		public virtual int param5 { get; set; }
		public abstract bool Can(Ctx ctx);
		public abstract void Do(Ctx ctx);
	}
	class AddStat : EffectFunc {
		int stat;
		int add;
		public override int param1 { get { return stat; } set { stat = value; } }
		public override int param2 { get { return add; } set { add = value; } }
		public override void Do(Ctx ctx) {
			throw new NotImplementedException();
		}

		public override bool Can(Ctx ctx) {
			throw new NotImplementedException();
		}
	}
}
