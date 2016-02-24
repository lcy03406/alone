//utf-8。
using System.Collections.Generic;

namespace Schema {
	public sealed class Entity : SchemaBase<Entity.ID, Entity> {
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Iact.A[] makes;
		public readonly Iact.A[] iacts;
		public readonly Play.AttrCreate attr;

		Entity(SpriteID sprite,
			string name,
			Iact.A[] makes,
			Iact.A[] iacts,
			Play.AttrCreate attr) {
			this.sprite = Sprite.GetA(sprite);
			this.name = name;
			this.makes = makes;
			this.iacts = iacts;
			this.attr = attr;
		}

		public enum ID {
			Human,
			Boulder,
			Tree_Pine,
			Tree_Oak,
			Workshop_Campfire,
		}
		static public void Init() {
			InitBoulder();
			InitTree();
			InitCreature();
			InitWorkshop();
		}

		static Iact.A[] boulder_makes = {
		};
		static Iact.A[] boulder_iacts = {
			Iact.GetA (Iact.ID.Chip_Stone),
		};
		static void InitBoulder() {
			Add(ID.Boulder, new Entity(
				sprite: SpriteID.b_mountain,
				name: "Boulder",
				makes: boulder_makes,
				iacts: boulder_iacts,
				attr: new Play.Ents.Tree(
					stat: null,
					part: new Play.Attrs.Grow() {
						items = {
							{ PartID.Boulder_Stone, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Stone),
								count: 100,
								cap: 100,
								q: 10,
								grow_span: 0,
								grow_count: 0
							)},
						}
					}
				)
			));
		}

		static Iact.A[] tree_makes = {
		};
		static Iact.A[] tree_iacts = {
			Iact.GetA (Iact.ID.Tree_PickBranch),
			Iact.GetA (Iact.ID.Tree_PickFruit),
		};
		static void InitTree() {
			Add(ID.Tree_Pine, new Entity(
				sprite: SpriteID.b_tree_pine,
				name: "Pine Tree",
				makes: tree_makes,
				iacts: tree_iacts,
                attr: new Play.Ents.Tree(
					stat: new Play.Attrs.Stat<Play.Stats.Tree>() {
						ints = {
							{ Play.Stats.Tree.Grouth, 0 },
						},
						caps = {
							{ Play.Stats.Tree.Grouth, 100 },
						}
					},
					part: new Play.Attrs.Grow() {
						items = {
							{ PartID.Tree_Branch, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Branch),
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 10,
								grow_count: 1
							)},
						}
					}
				)
			));
			Add(ID.Tree_Oak, new Entity(
				sprite: SpriteID.b_tree_oak,
				name: "Oak Tree",
				makes: tree_makes,
				iacts: tree_iacts,
				attr: new Play.Ents.Tree(
					stat: new Play.Attrs.Stat<Play.Stats.Tree>() {
						ints = {
							{ Play.Stats.Tree.Grouth, 0 },
						},
						caps = {
							{ Play.Stats.Tree.Grouth, 100 },
						}
					},
					part: new Play.Attrs.Grow() {
						items = {
							{ PartID.Tree_Branch, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Branch),
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 10,
								grow_count: 1
							)},
							{ PartID.Tree_Fruit, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.OakNut),
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 100,
								grow_count: 10
							)},
						}
					}
				)
			));
		}

		static Iact.A[] human_makes = {
			Iact.GetA(Iact.ID.Make_Cross),
			Iact.GetA(Iact.ID.Build_Campfire),
		};
		static Iact.A[] creature_iacts = {
			Iact.GetA(Iact.ID.Butcher_Meat),
			Iact.GetA(Iact.ID.Butcher_Bone),
		};
		static void InitCreature() {
			Add(ID.Human, new Entity(
				sprite: SpriteID.c_human_young,
				name: "Human",
				makes: human_makes,
				iacts: creature_iacts,
				attr: new Play.Ents.Creature(
					stat: new Play.Attrs.Stat<Play.Stats.Creature>() {
						ints = {
							{ Play.Stats.Creature.HitPoint, 10 },
							{ Play.Stats.Creature.Stamina, 10 },
						},
						caps = {
							{ Play.Stats.Creature.HitPoint, 10 },
							{ Play.Stats.Creature.Stamina, 10 },
						}
					},
					part: new Play.Attrs.Grow() {
						items = {
							{ PartID.Creature_Bone, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Branch), //TODO
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 0,
								grow_count: 0
							)},
							{ PartID.Creature_Meat, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Branch), //TODO
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 0,
								grow_count: 0
							)},
						}
					}
				)
			));
		}

		static Iact.A[] workshop_makes = {
		};
		static Iact.A[] workshop_iacts = {
		};
		static void InitWorkshop() {
			Add(ID.Workshop_Campfire, new Entity(
				sprite: SpriteID.b_volcano,
				name: "Campfire",
				makes: workshop_makes,
				iacts: workshop_iacts,
				attr: new Play.Ents.Workshop()
			));
		}
	}
}

/*
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
*/
