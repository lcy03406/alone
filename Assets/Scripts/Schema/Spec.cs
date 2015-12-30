//utf-8。
using System;

namespace Schema {

	//TODO
	public enum StatMeter {
		HitPoint, //
		Stamina,  //for hard work and battle
		Vitality, //for mental work

		Food,
		Water,
		Sleep,
		Fun,

		Size
	}

	public enum StatPoint {
		Strength, //Attack
		Constitution, //or Toughness HitPoint
		Agility, //Defence
		Dexterity, //crafting speed/quality
		Endurance, //StaminaPoint
				   //Willpower, //reduce pain
		Focus, //Accuracy
		Intelligence, //skill learning rate
		Creativity, //crafting quality
		Charisma, //magic
				  //Wisdom, Perception

		Size
	}

	public class Spec : SchemaBase<Spec.ID, Spec> {
		public readonly Sprite.A sprite;
		public readonly Type ai;
		private Spec (Schema.SpriteID spid, string ai) {
			this.sprite = Sprite.GetA (spid);
			this.ai = typeof(PlayAI).Module.GetType("PlayAI"+ai);
        }
		public enum ID {
			None,
			Human,
		}
		static public void Init () {
			Add (ID.Human, new Spec (Schema.SpriteID.c_human_young, "Human"));
		}
	}
}

public class Gait {
	[Flags]
	public enum ID {
		None = 0,
		Crawl = 1 << 0,
		Walk = 1 << 1,
		Run = 1 << 2,
		Sprint = 1 << 3,
		Swim = 1 << 4,
		Fly = 1 << 5,
	}
	int timePrepare;
	int timeRecover;
	int stamina;
}

public class Attack {
	int timePrepare;
	int timeRecover;
	int stamina;
	int tohit;
	int damage;
}

