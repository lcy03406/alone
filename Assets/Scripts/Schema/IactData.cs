//utf-8。
/*
namespace Schema {
	public sealed partial class Iact : SchemaBase<Iact.ID, Iact> {
		public enum ID {
			Rest,
			Attack_Punch,
			Travel_Down,
			Chip_Stone,
			Tree_PickBranch,
			Tree_PickFruit,
			Butcher_Meat,
			Butcher_Bone,
			Make_Cross,
			Make_Knife_Stone,
			Make_Axe_Stone,
			Make_Knife_Bone,
			Build_Campfire,
		}
		static public void Init() {
			InitCommon();
			InitAttack();
			InitTravel();
			InitBoulder();
			InitTree();
			InitCreature();
			InitMake();
			InitBuild();
		}

		static private void InitCommon() {
			Add(ID.Rest, Rest(name: "rest",
				time1: 1,
				sta: 1
			));
		}

		static private void InitAttack() {
			Add(ID.Attack_Punch, Attack("punch",
				time1: 3,
				time2: 0,
				sta: 1,
				damage: new Play.Calcs.GetStat(new Play.Calcs.Src(), StatID.Creature_Damage)
			));
		}

		static private void InitTravel() {
			Add(ID.Travel_Down, Travel("going down?",
				time1: 5,
				time2: 0,
				sta: 3, //TODO
				to: -1
			));
		}

		static private void InitBoulder() {
			Add(ID.Chip_Stone, Pick("chip stone",
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Boulder_Stone,
				count: 1
			));
		}

		static private void InitTree() {
			Add(ID.Tree_PickBranch, Pick("pick branch",
				time1: 3,
				time2: 0,
				sta: 1,
				part: PartID.Tree_Branch,
				count: 1
			));
			Add(ID.Tree_PickFruit, Pick("pick fruit",
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Tree_Fruit,
				count: 1
			));
		}

		static private void InitCreature() {
			Add(ID.Butcher_Meat, Butcher("cut meat",
				time1: 3,
				time2: 0,
				sta: 1,
				part: PartID.Creature_Meat,
				count: 1
			));
			Add(ID.Butcher_Bone, Butcher("cut bone",
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Creature_Bone,
				count: 1
			));
		}

		static private void InitMake() {
			Add(ID.Make_Cross, Make("make a cross",
				time1: 3,
				time2: 0,
				sta: 3,
				tools: new Play.ItemSelect[] {
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Branch), count: 2)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(ItemID.Cross),
					q_base: 0,
					q_from: new int[] {0},
					q_rand: 1,
					cap_from: null,
					count: 1)
				},
				build: null
			));
			Add(ID.Make_Knife_Stone, Make("make a stone knife",
				time1: 3,
				time2: 0,
				sta: 3,
				tools: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Stone), count: 1)
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Stone), count: 1)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(ItemID.Knife),
					q_base: 0,
					q_from: new int[] {0, 1},
					q_rand: 1,
					cap_from: null,
					count: 1)
				},
				build: null
			));
			Add(ID.Make_Axe_Stone, Make("make a stone axe",
				time1: 3,
				time2: 0,
				sta: 3,
				tools: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Knife), count: 1)
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Stone), count: 1),
					new Play.ItemSelect(a: Item.GetA(ItemID.Branch), count: 1)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(ItemID.Axe),
					q_base: 0,
					q_from: new int[] {0, 1},
					q_rand: 1,
					cap_from: null,
					count: 1)
				},
				build: null
			));
			Add(ID.Make_Knife_Bone, Make("make a bone knife",
				time1: 3,
				time2: 0,
				sta: 3,
				tools: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Knife), count: 1)
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Bone), count: 1),
					new Play.ItemSelect(a: Item.GetA(ItemID.Branch), count: 1)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(ItemID.Knife),
					q_base: 10,
					q_from: new int[] {0, 1},
					q_rand: 1,
					cap_from: null,
					count: 1)
				},
				build: null
			));
		}

		static private void InitBuild() {
			Add(ID.Build_Campfire, Make("build a campfire",
				time1: 0,
				time2: 10,
				sta: 3, //TODO
				tools: new Play.ItemSelect[] {
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(ItemID.Branch), count: 5)
				},
				products: null,
				build: Entity.GetA(EntityID.Workshop_Campfire)
			));
		}
	}
}
*/
