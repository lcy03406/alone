using System;
using Sprite = UnityEngine.Sprite;

namespace Edit {
	//TODO id
	public sealed class ATile : Meta {
		public string name;
		public Sprite sprite;

		public override void AfterLoad() {
			sprite = LoadSprite(name);
		}

		private static Sprite[] all_sprites;
		private static Sprite LoadSprite(string name) {
			if (all_sprites == null) {
				all_sprites = UnityEngine.Resources.LoadAll<UnityEngine.Sprite>("Sprites/tileset");
			}
			foreach (Sprite sprite in all_sprites) {
				if (sprite.name == name) {
					return sprite;
				}
			}
			throw new GameResourceException(string.Format("sprite not found. name = {0}", name));
		}
	}
}
