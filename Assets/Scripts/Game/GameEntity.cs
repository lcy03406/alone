//utf-8。
using UnityEngine;

using Play;

public class GameEntity : MonoBehaviour {

	Game game;
	Entity ent;

	void Start () {
	}
	
	void Update () {
		if (ent == null)
			return;
		Debug.Assert (ent.world != null);
		transform.localPosition = game.Pos (ent.c);
		Schema.SpriteID dirs = (Schema.SpriteID)((int)Schema.SpriteID.u_dir0 + (int)ent.dir);
		transform.FindChild ("Direction").GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (dirs).s.sprite;
		if (ent.isPlayer)
			return;
		Play.Show show = ent.GetAttr<Play.Show> ();
		if (show == null)
			return;
		GetComponent<SpriteRenderer> ().sprite = show.Sprite ().s.sprite;
	}
	
	public void Init (Game game, Entity ent) {
		this.game = game;
		this.ent = ent;
	}

	public void OnMove () {
	}
}
