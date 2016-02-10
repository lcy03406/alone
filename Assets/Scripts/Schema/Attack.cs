//utf-8。

namespace Schema {
	public sealed class Attack : SchemaBase<Attack.ID, Attack> {
		public Play.Attack i;

		public enum ID {
			None,
			Punch,
		}
		static public void Init () {
			Add (ID.Punch, new Attack { i = new Play.Creature.Punch () });
		}
	}
}
