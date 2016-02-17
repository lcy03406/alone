//utf-8。
using System;

namespace Schema {
	public sealed class Creature : SchemaBase<Creature.ID, Creature> {
		public readonly Sprite.A sprite;
		public readonly Play.Creature.Stat born_stat;
		public readonly Play.Creature.Stat renew_stat;
		public readonly Type ai;
		public readonly Iact.A[] know_make;
		private Creature (Schema.SpriteID sprite,
			Play.Creature.Stat born_stat,
			Play.Creature.Stat renew_stat,
			Type ai,
			Iact.A[] know_make)
		{
			this.sprite = Sprite.GetA (sprite);
			this.born_stat = born_stat;
			this.renew_stat = renew_stat;
			this.ai = ai;
			this.know_make = know_make;
        }
		public enum ID {
			None,
			Human,
			Tree_Pine,

		}
		static public void Init () {
			Add (ID.Human, new Creature (
				sprite: Schema.SpriteID.c_human_young,
				born_stat: new Play.Creature.Stat {
					ints = {
						{ Play.Creature.Stat.ID.HitPoint, 10 },
						{ Play.Creature.Stat.ID.Stamina, 10 },
					},
                    caps = {
						{ Play.Creature.Stat.ID.HitPoint, 10 },
						{ Play.Creature.Stat.ID.Stamina, 10 },
					}
				},
				renew_stat: new Play.Creature.Stat {
					ints = {
						{ Play.Creature.Stat.ID.HitPoint, 1 },
						{ Play.Creature.Stat.ID.Stamina, 1 },
					}
				},
				ai: typeof(Play.Creature.AIHuman),
				know_make: new Iact.A[] {
					Iact.GetA(Iact.ID.Make_Yeah),
				}
			));
        }
	}
}

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
