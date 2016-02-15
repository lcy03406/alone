//utf-8。

namespace Schema {
	public sealed class Iact : SchemaBase<Iact.ID, Iact> {
		public readonly Play.Iact i;

		private Iact(Play.Iact i) {
			this.i = i;
		}

		public enum ID {
			None,
			Attack_Punch,
			Tree_PickBranch,
			Tree_PickFruit,
			Make_Yeah,
		}
		static public void Init() {
			InitAttack();
			InitTree();
			InitMake();
		}

		static private void InitAttack() {
			Add(ID.Attack_Punch, new Iact(Play.Iacts.Attack(
				time1: 3,
				time2: 0,
				sta: 1,
				damage: new Play.Calcs.Stat<Play.Creature.Stat.ID>(new Play.Calcs.Src(), Play.Creature.Stat.ID.Damage)
			)));
		}

		static private void InitTree() {
			Add(ID.Tree_PickBranch, new Iact(i: Play.Iacts.Pick(
				time1: 3,
				time2: 0,
				sta: 1,
				st: Play.Tree.Stat.ID.Branch,
				part: Tree.Part.Branch,
				count: 1
			)));
			Add(ID.Tree_PickFruit, new Iact(i: Play.Iacts.Pick(
				time1: 5,
				time2: 0,
				sta: 1,
				st: Play.Tree.Stat.ID.Fruit,
				part: Tree.Part.Fruit,
				count: 1
			)));
		}

		static private void InitMake() {
			Add(ID.Make_Yeah, new Iact(i: Play.Iacts.Make(
				time1: 3,
				time2: 0,
				sta: 3, //TODO
				tools: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 1)
				},
				reagents: new Play.ItemSelect[] {
					new Play.ItemSelect(a: Item.GetA(Item.ID.Branch), count: 2)
				},
				products: new Play.ItemCreate[] {
					new Play.ItemCreate(a: Item.GetA(Item.ID.Yeah),
					q_base: 0,
					q_from: new int[] {1},
					q_rand: 1,
					cap_from: new int[] {0},
					count: 1)
				}
			)));
		}
	}
}
