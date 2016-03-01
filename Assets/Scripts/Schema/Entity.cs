//utf-8。
using System;
using System.Collections.Generic;

namespace Schema {
	public class Stage {
		public readonly Iact.A[] make;
		public readonly Iact.A[] iact;
		public readonly Play.Attrs.Stat<UsageID> usage;

		public Stage(Iact.A[] make, Iact.A[] iact, Play.Attrs.Stat<UsageID> usage) {
			this.make = make;
			this.iact = iact;
			this.usage = usage;
		}
	}
	public sealed class Entity : SchemaBase<Entity.ID, Entity> {
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Dictionary<Type, Stage> stages;
		public readonly Play.AttrCreate attr;

		Entity(SpriteID sprite,
			string name,
			Dictionary<Type, Stage> stages,
			Play.AttrCreate attr) {
			this.sprite = Sprite.GetA(sprite);
			this.name = name;
			this.stages = stages;
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

		static Dictionary<Type, Stage> boulder_stages = new Dictionary<Type, Stage> {
			{
				typeof(Play.Attrs.Stages.Static.Static),
				new Stage (
					make: null,
					iact: new Iact.A[] {
						Iact.GetA (Iact.ID.Chip_Stone),
					},
					usage: null
				)
			}
		};
		static void InitBoulder() {
			Add(ID.Boulder, new Entity(
				sprite: SpriteID.b_mountain,
				name: "boulder",
				stages: boulder_stages,
				attr: new Play.Ents.Static(
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

		static Dictionary<Type, Stage> tree_stages = new Dictionary<Type, Stage> {
			{
				typeof(Play.Attrs.Stages.Tree.Young),
				new Stage (
					make: null,
					iact: new Iact.A[] {
						Iact.GetA (Iact.ID.Tree_PickBranch),
						Iact.GetA (Iact.ID.Tree_PickFruit),
					},
					usage: null
				)
			},
            {
				typeof(Play.Attrs.Stages.Tree.Grown),
				new Stage (
					make: null,
					iact: new Iact.A[] {
						Iact.GetA (Iact.ID.Tree_PickBranch),
						Iact.GetA (Iact.ID.Tree_PickFruit),
					},
					usage: null
				)
			}
		};
		static void InitTree() {
			Add(ID.Tree_Pine, new Entity(
				sprite: SpriteID.b_tree_pine,
				name: "pine tree",
				stages: tree_stages,
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
				name: "oak tree",
				stages: tree_stages,
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

		static Dictionary<Type, Stage> human_stages = new Dictionary<Type, Stage> {
			{
				typeof(Play.Attrs.Stages.Creature.Alive),
				new Stage (
					make: new Iact.A[] {
						Iact.GetA(Iact.ID.Make_Cross),
						Iact.GetA(Iact.ID.Make_Knife_Stone),
						Iact.GetA(Iact.ID.Make_Axe_Stone),
						Iact.GetA(Iact.ID.Make_Knife_Bone),
						Iact.GetA(Iact.ID.Build_Campfire),
					},
					iact: null,
					usage: null
				)
			},
			{
				typeof(Play.Attrs.Stages.Creature.Dead),
				new Stage (
					make: null,
					iact: new Iact.A[] {
						Iact.GetA(Iact.ID.Butcher_Meat),
						Iact.GetA(Iact.ID.Butcher_Bone),
					},
					usage: null
				)
			}
		};
		static void InitCreature() {
			Add(ID.Human, new Entity(
				sprite: SpriteID.c_human_young,
				name: "human",
				stages: human_stages,
				attr: new Play.Ents.Creature(
					stat: new Play.Attrs.Stat<Play.Stats.Creature>() {
						ints = {
							{ Play.Stats.Creature.HitPoint, 10 },
							{ Play.Stats.Creature.Stamina, 10 },
							{ Play.Stats.Creature.Damage, 5 },
						},
						caps = {
							{ Play.Stats.Creature.HitPoint, 10 },
							{ Play.Stats.Creature.Stamina, 10 },
							{ Play.Stats.Creature.Damage, 5 },
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

		static Dictionary<Type, Stage> campfire_stages = new Dictionary<Type, Stage> {
			{
				typeof(Play.Attrs.Stages.Static.Static),
				new Stage (
					make: null,
					iact: null,
					usage: new Play.Attrs.Stat<UsageID>() {
						ints = {
							{ UsageID.Workshop_Cookfire, 1 }
						}
					}
				)
			}
		};
		static void InitWorkshop() {
			Add(ID.Workshop_Campfire, new Entity(
				sprite: SpriteID.b_volcano,
				name: "campfire",
				stages: campfire_stages,
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
