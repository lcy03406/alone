//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public sealed class AAttack : AAct {
		public int time_in;
		public int time_out;
		public int powerful;
		public int precise;

		//TODO log text

		public AAttack() {
			prepare.target = TargetSelect.Unit;
			prepare.distance = 2;
		}
	}
}
