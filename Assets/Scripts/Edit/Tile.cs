using System;

namespace Edit {
	//TODO id
	public sealed class Tile : Data {
		public UnityEngine.Sprite sprite;

		public static void Init(All all) {
			UnityEngine.Sprite[] sprites = UnityEngine.Resources.LoadAll<UnityEngine.Sprite>("Sprites/tileset");
			for (int i = 0; i < sprites.Length; ++i) {
				all.Add (i, new Tile() { sprite = sprites[i] });
			}
		}
	}
}
