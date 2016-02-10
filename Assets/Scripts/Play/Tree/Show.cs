//utf-8。
using System;

namespace Play.Tree {
	[Serializable]
	public class Show : Play.Show {
		public override Schema.Sprite.A Sprite () {
			Core core = ent.GetAttr<Core> ();
			return core.race.s.sprite;
		}
	}
}