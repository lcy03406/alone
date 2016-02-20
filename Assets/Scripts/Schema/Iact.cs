//utf-8。

namespace Schema {
	public sealed class Iact : SchemaBase<Iact.ID, Iact> {
		public readonly string name;
		public readonly Play.Iact i;

		private Iact(string name, Play.Iact i) {
			this.name = name;
			this.i = i;
		}

		public enum ID {
			Rest,
			Attack_Punch,
			Chip_Stone,
			Tree_PickBranch,
			Tree_PickFruit,
			Butcher_Meat,
			Butcher_Bone,
			Make_Cross,
			Build_Campfire,
		}
		static public void Init() {
			InitCommon();
			InitAttack();
			InitBoulder();
			InitTree();
			InitCreature();
			InitMake();
			InitBuild();
		}

		static private void InitCommon() {
			Add(ID.Rest, new Iact("rest", Play.Iacts.Rest(
				time1: 1,
				sta: 1
			)));
		}

		static private void InitAttack() {
			Add(ID.Attack_Punch, new Iact("punch", Play.Iacts.Attack(
				time1: 3,
				time2: 0,
				sta: 1,
				damage: new Play.Calcs.GetStat<Play.Stats.Creature>(new Play.Calcs.Src(), Play.Stats.Creature.Damage)
			)));
		}

		static private void InitBoulder() {
			Add(ID.Chip_Stone, new Iact("chip stone", Play.Iacts.Pick(
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Boulder_Stone,
				count: 1
			)));
		}

		static private void InitTree() {
			Add(ID.Tree_PickBranch, new Iact("pick branch", Play.Iacts.Pick(
				time1: 3,
				time2: 0,
				sta: 1,
				part: PartID.Tree_Branch,
				count: 1
			)));
			Add(ID.Tree_PickFruit, new Iact("pick fruit", Play.Iacts.Pick(
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Tree_Fruit,
				count: 1
			)));
		}

		static private void InitCreature() {
			Add(ID.Butcher_Meat, new Iact("cut meat", Play.Iacts.Butcher(
				time1: 3,
				time2: 0,
				sta: 1,
				part: PartID.Creature_Meat,
				count: 1
			)));
			Add(ID.Butcher_Bone, new Iact("cut bone", Play.Iacts.Butcher(
				time1: 5,
				time2: 0,
				sta: 1,
				part: PartID.Creature_Bone,
				count: 1
			)));
		}

		static private void InitMake() {
			Add(ID.Make_Cross, new Iact("make a cross", Play.Iacts.Make(
				time1: 3,
				time2: 0,
				sta: 3, //TODO
				tools: new Play.ItemSelect[] {
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 2)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(Item.ID.Cross),
					q_base: 0,
					q_from: new int[] {0},
					q_rand: 1,
					cap_from: null,
					count: 1)
				},
				build: null
			)));
		}

		static private void InitBuild() {
			Add(ID.Build_Campfire, new Iact("build a campfire", Play.Iacts.Make(
				time1: 0,
				time2: 10,
				sta: 3, //TODO
				tools: new Play.ItemSelect[] {
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 5)
				},
				products: null,
				build: new Play.EntityCreate(a: Entity.GetA(Entity.ID.Workshop_Campfire))
			)));
		}
	}
}
