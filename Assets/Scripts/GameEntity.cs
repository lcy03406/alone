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
		Schema.SpriteID dirs = (Schema.SpriteID)((int)Schema.SpriteID.u_dir0 + (int)ent.d.dir);
		transform.FindChild ("Direction").GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (dirs).s.sprite;
		if (ent.isPlayer)
			return;
		if (ent.d.stat.hp <= 0) {
			GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (Schema.SpriteID.d_shovel).s.sprite;
			return;
		} else if (ent.d.act != null) {
			//TODO
			if (ent.d.act is PlayActMove) {
				GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (Schema.SpriteID.d_boots).s.sprite;
			} else if (ent.d.act is PlayActAttack) {
				GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (Schema.SpriteID.d_gauntlets).s.sprite;
			} else {
				GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (Schema.SpriteID.d_helm).s.sprite;
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
