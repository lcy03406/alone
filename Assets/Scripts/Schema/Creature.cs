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

	public sealed class Creature : SchemaBase<Creature.ID, Creature> {
		public readonly Sprite.A sprite;
		public readonly Type ai;
		private Creature (Schema.SpriteID spid, Type ai) {
			this.sprite = Sprite.GetA (spid);
			this.ai = ai;
        }
		public enum ID {
			None,
			Human,
			Tree_Pine,

		}
		static public void Init () {
			Add (ID.Human, new Creature (Schema.SpriteID.c_human_young, typeof(Play.Creature.AIHuman)));
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

