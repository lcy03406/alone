using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour {

	Game game;
	WorldEntity ent;

	void Start () {
	}
	
	void Update () {
		if (ent == null)
			return;
		Debug.Assert (ent.world != null);
		transform.localPosition = game.Pos (ent.d.c);
		SchemeSpriteID dirs = (SchemeSpriteID)((int)SchemeSpriteID.u_dir0 + (int)ent.d.dir);
		transform.FindChild ("Direction").GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(dirs);
		if (ent.isPlayer)
			return;
		if (ent.d.stat.hp <= 0) {
			GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_shovel);
			return;
		} else if (ent.d.act != null) {
			//TODO
			if (ent.d.act is PlayActMove) {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_boots);
			} else if (ent.d.act is PlayActAttack) {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_gauntlets);
			} else {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_helm);
			}
		}
	}
	
	public void Init (Game game, WorldEntity ent) {
		this.game = game;
		this.ent = ent;
	}

	public void OnMove () {
	}
}
