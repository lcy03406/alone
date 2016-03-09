//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Play;

public class Game : MonoBehaviour, World.View {

	public GameObject playerPrefab;
	public GameObject tilePrefab;

	public static Game game;

	const int VIEW_RADIUS = 32;

	public World world;
	public Entity player;
	[HideInInspector]
	public Transform root;

	Coord offset = new Coord();

	// Use this for initialization
	void Start () {
		Assert.raiseExceptions = true;
		game = this;
		root = new GameObject ("GameRoot").transform;
		world = new World ();
		world.SetView (this);
		world.LoadWorld (Application.persistentDataPath + "/save/", "default");
	}

	void OnDestroy () {
		world.SaveWorld ();
	}

	void FixedUpdate () {
		world.Update ();
	}

	public Vector3 Pos (Coord c) {
		return new Vector3 (c.x - offset.x, c.y - offset.y, 0);
	}

	void World.View.OnGridLoad (Coord g, Grid grid) {
		string gname = string.Format("Layer_{0}_Grid_{1}", world.param.layer, g);
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
					o.name = string.Format ("Layer_{0}_Tile_{1}_{2}", world.param.layer, g.x + x, g.y + y);
                    o.GetComponent<SpriteRenderer> ().sprite = t.s.sprite.s.sprite;
					o.transform.SetParent(got);
					o.transform.localPosition = new Vector3(x, y, 0);
				}
			}
		}
	}

	void World.View.OnGridUnload (Coord g) {
		string gname = string.Format("Layer_{0}_Grid_{1}", world.param.layer, g);
		GameObject go = GameObject.Find (gname);
		if (go != null) {
			Destroy (go);
		}
	}

	void World.View.OnEntityAdd (Entity ent) {
		if (ent.isPlayer) {
			player = ent;
			GameObject o = Instantiate(playerPrefab);
			o.name = string.Format("Layer_{0}_Entity_{1}", ent.layer.z, ent.id);
			GameObject.Find("Main Camera").transform.SetParent(o.transform);
			UpdateEntity(o, ent);
		} else {
			ulong g = ent.id.value / 1024;
			string gname = string.Format("Layer_{0}_EntityGroup_{1}", ent.layer.z, g);
			GameObject go = GameObject.Find(gname);
			if (go == null) {
				go = new GameObject(gname);
				go.transform.SetParent(root);
			}
			GameObject o = Instantiate(playerPrefab);
			o.name = string.Format("Layer_{0}_Entity_{1}", ent.layer.z, ent.id);
			o.transform.SetParent(go.transform);
			UpdateEntity(o, ent);
		}
	}

	void World.View.OnEntityDel (Entity ent) {
		if (ent.isPlayer) {
			GameObject.Find("Main Camera").transform.SetParent(root.transform);
			string name = string.Format("Layer_{0}_Entity_{1}", ent.layer.z, ent.id);
			GameObject o = GameObject.Find(name);
			Destroy(o);
			player = null;
		} else {
			ulong g = ent.id.value / 1024;
			string gname = string.Format("Layer_{0}_EntityGroup_{1}", ent.layer.z, g);
			GameObject go = GameObject.Find(gname);
			if (go == null)
				return;
			string name = string.Format("Layer_{0}_Entity_{1}", ent.layer.z, ent.id);
			GameObject o = GameObject.Find(name);
			Destroy(o);
			if (go.transform.childCount == 0)
				Destroy(go);
		}
	}

	void World.View.OnEntityUpdate(Entity ent) {
		if (ent == null)
			return;
		if (ent.layer == null)
			return;
		string name = string.Format("Layer_{0}_Entity_{1}", ent.layer.z, ent.id);
		GameObject o = GameObject.Find(name);
		Assert.IsNotNull(o);
		if (o == null) {
			return;
		}
		UpdateEntity(o, ent);
	}

	static void UpdateEntity(GameObject o, Entity ent) {
		Play.Attrs.Pos pos = ent.GetAttr<Play.Attrs.Pos>();
		o.transform.localPosition = game.Pos(pos.c);
		Schema.SpriteID dirs = (Schema.SpriteID)((int)Schema.SpriteID.u_dir0 + (int)pos.dir);
		o.transform.FindChild("Direction").GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA(dirs).s.sprite;
		if (ent.isPlayer) {
			//TODO
			o.GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA(Schema.SpriteID.c_human_strong).s.sprite;
			return;
		}
		Play.Attrs.Core show = ent.GetAttr<Play.Attrs.Core>();
		if (show == null)
			return;
		o.GetComponent<SpriteRenderer>().sprite = show.GetSprite().s.sprite;
	}
}
