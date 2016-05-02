//utf-8。
using System;
using System.Collections.Generic;
using Edit;
using Play;
using Ctx = Play.Ctx;

namespace Fun {
	//TODO
	public interface EffectFunc {
		void TakeEffect(Ctx ctx, List<int> param);
	}
	class AddStat : EffectFunc {
		void EffectFunc.TakeEffect(Ctx ctx, List<int> param) {
			throw new NotImplementedException();
		}
	}
}
