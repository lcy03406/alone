using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class Game : MonoBehaviour, World.View {

	public GameObject playerPrefab;
	public GameObject tilePrefab;

	const int VIEW_RADIUS = 32;

	public World world;
	public Transform root;

	PlayCtrl ctrl;
	Coord offset = new Coord();

	// Use this for initialization
	void Start () {
		Assert.raiseExceptions = true;
		root = new GameObject ("GameRoot").transform;
		world = new World ();
		world.SetView (this);
		world.LoadWorld (Application.persistentDataPath + "/save/", "default");
	}

	void OnDestroy () {
		world.SaveWorld ();
	}

	// Update is called once per frame
	void Update () {
		UpdateInput ();
		world.Update ();
	}

	void UpdateInput () {
		if (ctrl == null)
			return;
		if (Input.anyKeyDown) {
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				ctrl.CmdAttack ();
				return;
			}
			int dx = 0;
			int dy = 0;
			if (Input.GetKeyDown (KeyCode.A)) {
				dx--;
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				dy--;
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				dx++;
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				dy++;
			}
			if (dx != 0 || dy != 0) {
				Direction to = new Coord (dx, dy).ToDirection ();
				ctrl.CmdMove (to);
				return;
			}
			if (Input.GetKeyDown (KeyCode.Period)) {
				//ctrl.CmdWait ();
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				ctrl.ListCanDo ();
			}
		}
	}

	public Vector3 Pos (Coord c) {
		return new Vector3 (c.x - offset.x, c.y - offset.y, 0);
	}

	void World.View.OnLoadGrid (Coord g, WorldGrid grid) {
		string gname = "GameGrid_" + g;
		GameObject go = GameObject.Find (gname);
		if (go == null) {
			go = new GameObject (gname);
			Transform got = go.transform;
			got.SetParent (root);
			got.localPosition = Pos (g);
			for (int x = 0; x < grid.d.tiles.GetLength(0); ++x) {
				for (int y = 0; y < grid.d.tiles.GetLength(1); ++y) {
					Schema.Floor.A t = grid.d.tiles[x,y];
					GameObject o = Instantiate(tilePrefab);
					o.name = string.Format ("GameTile_{0}_{1}", g.x + x, g.y + y);
                    o.GetComponent<SpriteRenderer> ().sprite = t.s.sprite.s.sprite;
					o.transform.SetParent(got);
					o.transform.localPosition = new Vector3(x, y, 0);
				}
			}
		}
	}

	void World.View.OnUnloadGrid (Coord g) {
		string gname = "GameGrid_" + g;
		GameObject go = GameObject.Find (gname);
		if (go != null) {
			Destroy (go);
		}
	}

	void World.View.OnLoadPlayer (WorldEntity wp) {
		this.ctrl = (PlayCtrl) wp.d.ai;
		GameObject player = Instantiate(playerPrefab);
		player.name = "GameEntity_" + wp.d.id;
		GameObject.Find ("Main Camera").transform.SetParent (player.transform);
		GameEntity ge = player.GetComponent<GameEntity> ();
		ge.Init (this, wp);
	}

	void World.View.OnAddEntity (WorldEntity ent) {
		ulong g = ent.d.id.value / 1024;
		string gname = "GameEntityGroup_" + g;
		GameObject go = GameObject.Find (gname);
		if (go == null) {
			go = new GameObject (gname);
			go.transform.SetParent (root);
		}
		GameObject o = Instantiate(playerPrefab);
		o.name = "GameEntity_" + ent.d.id;
		o.transform.SetParent (go.transform);
		GameEntity ge = o.GetComponent<GameEntity> ();
		ge.Init (this, ent);
	}

	void World.View.OnDelEntity (WorldEntity ent) {
		ulong g = ent.d.id.value / 1024;
		string gname = "GameEntityGroup_" + g;
		GameObject go = GameObject.Find (gname);
		if (go == null)
			return;
		string name = "GameEntity_" + ent.d.id;
		GameObject o = GameObject.Find (name);
		Destroy (o);
		if (go.transform.childCount == 0)
			Destroy (go);
	}
}
