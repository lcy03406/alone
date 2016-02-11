//utf-8。
using System;

namespace Play.Creature {
	[Serializable]
	public class Show : Play.Show {
		public override Schema.Sprite.A Sprite () {
			Core core = ent.GetAttr<Core> ();
			Stat stat = ent.GetAttr<Stat> ();
			//TODO
			if (stat.hp <= 0) {
				return Schema.Sprite.GetA (Schema.SpriteID.d_shovel);
			}
			Play.Creature.Act act = core.act;
			if (act != null) {
				//TODO
				if (act is Play.Creature.ActMove) {
					return Schema.Sprite.GetA (Schema.SpriteID.d_boots);
				} else if (act is Play.Creature.ActAttack) {
					return Schema.Sprite.GetA (Schema.SpriteID.d_gauntlets);
				} else {
					return Schema.Sprite.GetA (Schema.SpriteID.d_helm);
				}
			}
			return core.race.s.sprite;
		}
	}
}