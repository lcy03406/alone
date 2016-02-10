//utf-8。
using System;

namespace Play.Tree {
	public class PickBranch : Iact {
		public override bool Can (Entity src, Entity dst) {
			Core dstcore = dst.GetAttr<Core> ();
			if (dstcore == null)
				return false;
			return true;
		}

		public override void Interact (Entity src, Entity dst) {
			throw new NotImplementedException ();
		}
	}

	public class PickFruit : Iact {
		public override bool Can (Entity src, Entity dst) {
			Core dstcore = dst.GetAttr<Core> ();
			if (dstcore == null)
				return false;
			return true;
		}

		public override void Interact (Entity src, Entity dst) {
			throw new NotImplementedException ();
		}
	}
}
