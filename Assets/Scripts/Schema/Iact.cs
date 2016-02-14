//utf-8。

namespace Schema {
	public sealed class Iact : SchemaBase<Iact.ID, Iact> {
		public readonly Play.Iact i;

		private Iact(Play.Iact i) {
			this.i = i;
		}

		public enum ID {
			None,
			Tree_PickBranch,
			Tree_PickFruit,
		}
		static public void Init () {
			Add(ID.Tree_PickBranch, new Iact(i: new Play.Tree.IaPick(
				time1: 3,
				time2: 0,
				sta: 1,
				st: Play.Tree.Stat.ID.Branch,
				part: Tree.Part.Branch,
				count: 1
			)));
			Add(ID.Tree_PickFruit, new Iact(i: new Play.Tree.IaPick(
				time1: 5,
				time2: 0,
				sta: 1,
				st: Play.Tree.Stat.ID.Fruit,
				part: Tree.Part.Fruit,
				count: 1
			)));
		}
	}
}
