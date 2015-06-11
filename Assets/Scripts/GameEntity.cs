using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour {

	Game game;
	WorldEntity ent;

	void Start () {
	}
	
	void Update () {
	}
	
	public void Init (Game game, WorldEntity ent) {
		this.game = game;
		this.ent = ent;
		transform.SetParent (game.root);
		OnMove ();
	}

	public void OnMove () {
		transform.localPosition = game.Pos (ent.d.c);
		if (ent.isPlayer)
			return;
		if (ent.d.stat.hp <= 0) {
			GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_shovel);
		} else if (ent.d.act != null) {
			//TODO
			if (ent.d.act is PlayActMove) {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_boots);
			} else if (ent.d.act is PlayActAttack) {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_sword);
			} else {
				GetComponent<SpriteRenderer>().sprite = game.world.scheme.GetSprite(SchemeSpriteID.d_helm);
			}
		}
	}
}
