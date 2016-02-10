//utf-8。

namespace Schema {
	public sealed class Iact : SchemaBase<Iact.ID, Iact> {
		public Play.Iact i;

		public enum ID {
			None,
			Tree_PickBranch,
			Tree_PickFruit,
		}
		static public void Init () {
			Add (ID.Tree_PickBranch, new Iact { i = new Play.Tree.PickBranch () });
			Add (ID.Tree_PickFruit, new Iact { i = new Play.Tree.PickFruit () });
		}
	}
}
