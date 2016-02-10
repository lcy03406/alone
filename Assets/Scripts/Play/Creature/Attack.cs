//utf-8。

namespace Play.Creature {
	public class Punch : Attack {
		public override bool Can (Entity src, Entity dst) {
			Core dstcore = dst.GetAttr<Core> ();
			if (dstcore == null)
				return false;
			return true;
		}

		public override void Damage (Entity src, Entity dst) {
			Stat dststat = dst.GetAttr<Stat> ();
			if (dststat == null)
				return;
			dststat.hp -= 1;
		}
	}
}
