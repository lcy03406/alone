//utf-8。
using UnityEngine;

using Play;

public class GameEntity : MonoBehaviour {

	private int update_time = -1;
	Entity ent;

	void Start () {
	}
	
	void Update () {
		if (ent == null)
			return;
		Play.Attrs.Pos pos = ent.GetAttr<Play.Attrs.Pos>();
		int cur_time = Game.game.world.param.time;
		//if (update_time >= cur_time)
		//	return;
		update_time = cur_time;
		Debug.Assert (ent.world != null);
		transform.localPosition = Game.game.Pos (pos.c);
		Schema.SpriteID dirs = (Schema.SpriteID)((int)Schema.SpriteID.u_dir0 + (int)pos.dir);
		transform.FindChild ("Direction").GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA (dirs).s.sprite;
		if (ent.isPlayer) {
			//TODO
			GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA(Schema.SpriteID.c_human_strong).s.sprite;
			return;
		}
		Play.Attrs.Core show = ent.GetAttr<Play.Attrs.Core> ();
		if (show == null)
			return;
		GetComponent<SpriteRenderer>().sprite = show.GetSprite().s.sprite;
	}
	
	public void Init (Game game, Entity ent) {
        this.ent = ent;
	}

	public void OnMove () {
	}
}
