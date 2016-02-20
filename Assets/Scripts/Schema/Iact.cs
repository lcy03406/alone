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
			Tree_PickBranch,
			Tree_PickFruit,
			Make_Cross,
			Build_Campfire,
		}
		static public void Init() {
			InitCommon();
			InitAttack();
			InitTree();
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

		static private void InitTree() {
			Add(ID.Tree_PickBranch, new Iact("pick branch", Play.Iacts.Pick(
				time1: 3,
				time2: 0,
				sta: 1,
				st: Play.Stats.Tree.Branch,
				part: Play.Parts.Tree.Branch,
				count: 1
			)));
			Add(ID.Tree_PickFruit, new Iact("pick fruit", Play.Iacts.Pick(
				time1: 5,
				time2: 0,
				sta: 1,
				st: Play.Stats.Tree.Fruit,
				part: Play.Parts.Tree.Fruit,
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
