using System;
using UnityEngine;

public partial class Scheme {

	private Sprite[] sprites;

	public Sprite GetSprite (SchemeSpriteID sid) {
		return sprites [(int)sid];
	}

	private void LoadSprites () {
		sprites = Resources.LoadAll<Sprite> ("Sprites/tileset");
	}
	
	public void LoadAll() {
		LoadSprites ();
		LoadFloors ();
		LoadItems ();
	}

}

