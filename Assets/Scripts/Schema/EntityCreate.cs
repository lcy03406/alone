//utf-8。
using System;

namespace Schema {
	public sealed class EntityCreate : SchemaBase<EntityCreate.ID, EntityCreate> {
		public readonly Play.EntityCreate cre;

		private EntityCreate(Play.EntityCreate cre) {
			this.cre = cre;
        }

		public enum ID {
			Human,
			Tree_Pine,

		}
		static public void Init () {
			Add(ID.Human, new EntityCreate(new Play.Ents.Creature(
				a: Entity.GetA(Entity.ID.Human),
				stat: new Play.Attrs.Stat<Play.Stats.Creature>() {
					ints = {
						{ Play.Stats.Creature.HitPoint, 10 },
						{ Play.Stats.Creature.Stamina, 10 },
					},
                    caps = {
						{ Play.Stats.Creature.HitPoint, 10 },
						{ Play.Stats.Creature.Stamina, 10 },
					}
				}
			)));
			Add(ID.Tree_Pine, new EntityCreate(new Play.Ents.Tree(
				a: Entity.GetA(Entity.ID.Tree_Pine),
				stat: new Play.Attrs.Stat<Play.Stats.Tree>() {
					ints = {
						{ Play.Stats.Tree.Branch, 5 },
						{ Play.Stats.Tree.Fruit, 0 },
					},
					caps = {
						{ Play.Stats.Tree.Branch, 5 },
						{ Play.Stats.Tree.Fruit, 10 },
					}
				},
				part: new Play.Attrs.Part<Play.Parts.Tree>() {
					items = {
						{ Play.Parts.Tree.Branch, Item.GetA(Item.ID.Branch) },
                        { Play.Parts.Tree.Fruit, Item.GetA(Item.ID.Apple) } //TODO
					}
				}
			)));
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
