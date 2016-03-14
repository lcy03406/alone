//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public class EntityStage {
		public readonly Iact.A[] iact_src;
		public readonly Iact.A[] iact_dst;
		public readonly Iact.A[] make;
		public readonly Play.Attrs.Stat usage;

		public EntityStage(Iact.A[] iact_src, Iact.A[] iact_dst,
			Iact.A[] make, Play.Attrs.Stat usage) {
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

	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Stages stages;
		public readonly Stage.A start_stage;
		public readonly Play.Attrs.Stat stat;
		public readonly Play.Attrs.Part part;
		public readonly Play.AttrCreate attr;

		Entity(SpriteID sprite,
			string name,
			Stages stages,
			Stage.A start_stage,
			Play.Attrs.Stat stat,
			Play.Attrs.Part part,
			Play.AttrCreate attr) {
			this.sprite = Sprite.GetA(sprite);
			this.name = name;
			this.stages = stages;
			this.start_stage = start_stage;
			this.stat = stat;
			this.part = part;
			this.attr = attr;
		}
	}
}
/*
		public enum ID {
			Human,
			Boulder,
			Tree_Pine,
			Tree_Oak,
			Workshop_Mine,
			Workshop_Campfire,
		}
		static public void Init() {
			InitBoulder();
			InitTree();
			InitCreature();
			InitWorkshop();
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
				stat: new Play.Attrs.Stat() {
					ints = {
						{ StatID.Tree_Grouth, new Play.Attrs.Stat.St(
							value: 0,
							cap: 100
						)},
					},
				},
				part: new Play.Attrs.Part() {
					items = {
						{ PartID.Tree_Branch, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.Branch),
							count: 10,
							cap: 10,
							q: 10,
							grow_span: 500,
							grow_count: 1
						)},
						{ PartID.Tree_Fruit, new Play.Attrs.Part.PartItem(
							a: null,
							count: 0,
							cap: 0,
							q: 0,
							grow_span: 0,
							grow_count: 0
						)},
					}
				},
				attr: null
			));
			Add(ID.Tree_Oak, new Entity(
				sprite: SpriteID.b_tree_oak,
				name: "oak tree",
				stages: tree_stages,
				start_stage: Stage.GetA(Stage.ID.Tree_Young),
				stat: new Play.Attrs.Stat() {
					ints = {
						{ StatID.Tree_Grouth, new Play.Attrs.Stat.St(
							value: 0,
							cap: 100
						)},
					},
				},
				part: new Play.Attrs.Part() {
					items = {
						{ PartID.Tree_Branch, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.Branch),
							count: 10,
							cap: 10,
							q: 10,
							grow_span: 500,
							grow_count: 1
						)},
						{ PartID.Tree_Fruit, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.OakNut),
							count: 10,
							cap: 10,
							q: 10,
							grow_span: 1000,
							grow_count: 10
						)},
					}
				},
				attr: null
			));
		}

		static void InitWorkshop() { //TODO
			Stages mine_stages = new Stages {
				{
					Stage.ID.Workshop_On,
					new EntityStage(
						iact_src: null,
						iact_dst: new Iact.A[] {
							Iact.GetA(Iact.ID.Travel_Down),
						},
						make: null,
						usage: null
					)
				},
			};
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
						usage: new Play.Attrs.Stat() {
							ints = {
								{ StatID.Workshop_Cookfire, new Play.Attrs.Stat.St(
									value: 1,
									cap: 0
								)},
							}
						}
					)
				},
			};
			Add(ID.Workshop_Mine, new Entity(
				sprite: SpriteID.b_volcano_dead,
				name: "mine entrance",
				stages: mine_stages,
				start_stage: Stage.GetA(Stage.ID.Workshop_On),
				stat: null,
				part: null,
				attr: null
			));
			Add(ID.Workshop_Campfire, new Entity(
				sprite: SpriteID.b_volcano,
				name: "campfire",
				stages: campfire_stages,
				start_stage: Stage.GetA(Stage.ID.Workshop_On), //TODO
				stat: null,
				part: null,
				attr: null
			));
		}
	}
}
*/

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
