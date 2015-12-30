using System;

namespace Schema {
	public class Floor : SchemaBase<Floor.ID, Floor> {
		public readonly Sprite.A sprite;
        private Floor (Schema.SpriteID spid) {
			this.sprite = Sprite.GetA (spid);
		}
		public enum ID {
			None,
			Grass,
			Dirt,
			Ocean,
		}
		static public void Init () {
			Add (ID.Grass, new Floor (Schema.SpriteID.a_grass));
			Add (ID.Dirt,  new Floor (Schema.SpriteID.a_dirt));
			Add (ID.Ocean, new Floor (Schema.SpriteID.a_ocean));
		}
	}
}
