using System;

namespace Schema {
	public sealed class Sprite : SchemaBase<SpriteID, Sprite> {
		public readonly UnityEngine.Sprite sprite;

		private Sprite (UnityEngine.Sprite sprite) {
			this.sprite = sprite;
		}

		//static public implicit operator UnityEngine.Sprite (Sprite s) {
		//	return s.sprite;
		//}

		static private UnityEngine.Sprite[] sprites;

		//static public UnityEngine.Sprite GetSprite (Schema.SpriteID sid) {
		//	return sprites[(int)sid];
		//}

		static public void Init () {
			sprites = UnityEngine.Resources.LoadAll<UnityEngine.Sprite> ("Sprites/tileset");

			for (int i = 0; i < sprites.Length; ++i) {
				Add ((Schema.SpriteID)i, new Sprite (sprites[i]));
			}
		}
	}
}
