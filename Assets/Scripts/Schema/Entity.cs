//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public class EntityStage {
		public readonly Iact.A[] iact_src;
		public readonly Iact.A[] iact_dst;
		public readonly Iact.A[] make;
		public readonly Play.Attrs.Stat<UsageID> usage;

		public EntityStage(Iact.A[] iact_src, Iact.A[] iact_dst,
			Iact.A[] make, Play.Attrs.Stat<UsageID> usage) {
			this.iact_src = iact_src;
			this.iact_dst = iact_dst;
			this.make = make;
			this.usage = usage;
		}

		static bool HasIact(Iact.A[] iact, Iact.A a) {
			if (iact == null)
				return false;
			foreach (Iact.A i in iact) {
				if (i == a)
					return true;
			}
			return false;
		}

		public bool HasIactSrc(Iact.A a) {
			return HasIact(iact_src, a);
		}
		public bool HasIactDst(Iact.A a) {
			return HasIact(iact_dst, a);
		}
		public bool HasMake(Iact.A a) {
			return HasIact(make, a);
		}
	}

	public sealed class Entity : SchemaBase<Entity.ID, Entity> {
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Stages stages;
        public readonly Stage.A start_stage;
		public readonly Play.AttrCreate attr;

		Entity(SpriteID sprite,
			string name,
			Stages stages,
            Stage.A start_stage,
			Play.AttrCreate attr) {
			this.sprite = Sprite.GetA(sprite);
			this.name = name;
			this.stages = stages;
			this.start_stage = start_stage;
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

		static void InitBoulder() {
			Stages boulder_stages = new Stages {
				{
					Stage.ID.Boulder_Static,
					new EntityStage(
						iact_src: null,
						iact_dst: new Iact.A[] {
							Iact.GetA (Iact.ID.Chip_Stone),
						},
						make: null,
						usage: null
					)
				}
			};
            Add(ID.Boulder, new Entity(
				sprite: SpriteID.b_mountain,
				name: "boulder",
				stages: boulder_stages,
                start_stage: Stage.GetA(Stage.ID.Boulder_Static),
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

		static void InitTree() {
			Iact.A[] tree_iact = new Iact.A[] {
				Iact.GetA (Iact.ID.Tree_PickBranch),
				Iact.GetA (Iact.ID.Tree_PickFruit),
			};
			Stages tree_stages = new Stages {
				{
					Stage.ID.Tree_Young,
					new EntityStage(
						iact_src: null,
						iact_dst: tree_iact,
						make: null,
						usage: null
                    )
				},
                {
					Stage.ID.Tree_Grown,
					new EntityStage(
						iact_src: null,
						iact_dst: tree_iact,
						make: null,
						usage: null
					)
				}
			};
			Add(ID.Tree_Pine, new Entity(
				sprite: SpriteID.b_tree_pine,
				name: "pine tree",
				stages: tree_stages,
				start_stage: Stage.GetA(Stage.ID.Tree_Young),
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
								grow_span: 500,
								grow_count: 1
							)},
							{ PartID.Tree_Fruit, new Play.Attrs.Grow.Part(
								a: null,
								count: 0,
								cap: 0,
								q: 0,
								grow_span: 0,
								grow_count: 0
							)},
						}
					}
				)
			));
			Add(ID.Tree_Oak, new Entity(
				sprite: SpriteID.b_tree_oak,
				name: "oak tree",
				stages: tree_stages,
				start_stage: Stage.GetA(Stage.ID.Tree_Young),
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
								grow_span: 500,
								grow_count: 1
							)},
							{ PartID.Tree_Fruit, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.OakNut),
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 1000,
								grow_count: 10
							)},
						}
					}
				)
			));
		}

		static void InitCreature() {
			Iact.A[] human_make = new Iact.A[] {
				Iact.GetA(Iact.ID.Make_Cross),
				Iact.GetA(Iact.ID.Make_Knife_Stone),
				Iact.GetA(Iact.ID.Make_Axe_Stone),
				Iact.GetA(Iact.ID.Make_Knife_Bone),
				Iact.GetA(Iact.ID.Build_Campfire),
			};
			Iact.A[] human_iact_src = new Iact.A[] {
				Iact.GetA(Iact.ID.Rest),
				Iact.GetA(Iact.ID.Attack_Punch),
				Iact.GetA(Iact.ID.Chip_Stone),
				Iact.GetA(Iact.ID.Tree_PickBranch),
				Iact.GetA(Iact.ID.Tree_PickFruit),
				Iact.GetA(Iact.ID.Butcher_Meat),
				Iact.GetA(Iact.ID.Butcher_Bone),
				Iact.GetA(Iact.ID.Make_Cross),
				Iact.GetA(Iact.ID.Make_Knife_Stone),
				Iact.GetA(Iact.ID.Make_Axe_Stone),
				Iact.GetA(Iact.ID.Make_Knife_Bone),
				Iact.GetA(Iact.ID.Build_Campfire),
			};
			Iact.A[] creature_iact_dst = new Iact.A[] {
				Iact.GetA(Iact.ID.Butcher_Meat),
				Iact.GetA(Iact.ID.Butcher_Bone),
			};
			Stages human_stages = new Stages {
				{
					Stage.ID.Creature_Alive,
					new EntityStage(
						iact_src: human_iact_src,
						iact_dst: null,
						make: human_make,
						usage: null
					)
				},
				{
					Stage.ID.Creature_Dead,
					new EntityStage(
						iact_src: null,
						iact_dst: creature_iact_dst,
						make: null,
						usage: null
					)
				}
 			};
			Add(ID.Human, new Entity(
				sprite: SpriteID.c_human_young,
				name: "human",
				stages: human_stages,
				start_stage: Stage.GetA(Stage.ID.Creature_Alive),
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
								a: Item.GetA(Item.ID.Bone),
								count: 10,
								cap: 10,
								q: 10,
								grow_span: 0,
								grow_count: 0
							)},
							{ PartID.Creature_Meat, new Play.Attrs.Grow.Part(
								a: Item.GetA(Item.ID.Meat),
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

		static void InitWorkshop() { //TODO
			Stages campfire_stages = new Stages {
				{
					Stage.ID.Workshop_Off,
					new EntityStage(
						iact_src: null,
						iact_dst: null,
						make: null,
						usage: null
					)
				},
				{
					Stage.ID.Workshop_On,
					new EntityStage(
						iact_src: null,
						iact_dst: null,
						make: null,
						usage: new Play.Attrs.Stat<UsageID>() {
							ints = {
								{ UsageID.Workshop_Cookfire, 1 }
							}
						}
					)
				},
 			};
			Add(ID.Workshop_Campfire, new Entity(
				sprite: SpriteID.b_volcano,
				name: "campfire",
				stages: campfire_stages,
				start_stage: Stage.GetA(Stage.ID.Workshop_On), //TODO
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
