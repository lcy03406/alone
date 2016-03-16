//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Play;

public class Game : MonoBehaviour, World.View {

	public GameObject playerPrefab;
	public GameObject gridPrefab;

	public static Game game;

	const int VIEW_RADIUS = 32;

	public World world;
	public Entity player;
	[HideInInspector]
	public Transform root;
	[HideInInspector]
	public int world_update_time;

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
		if (world.Update()) {
			world_update_time = world.param.time;
		}
	}

	public Vector3 Pos (Coord c) {
		return new Vector3 (c.x - offset.x, c.y - offset.y, 0);
	}

	void World.View.OnGridLoad (Coord g, Grid grid) {
		string gname = string.Format("Layer_{0}_Grid_{1}", world.param.layer, g);
		GameObject go = GameObject.Find (gname);
		if (go == null) {
			go = Instantiate(gridPrefab);
			go.name = gname;
			Transform got = go.transform;
			got.SetParent (root);
			got.localPosition = Pos (g) + new Vector3(-0.5f, -0.5f, 1);
			MeshFilter mf = go.GetComponent<MeshFilter>();
			mf.mesh.uv = GenerateUV(grid.d.tiles);
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
			Transform cama = GameObject.Find("Main Camera").transform;
			cama.SetParent(o.transform);
			cama.localPosition = new Vector3(0, 0, cama.localPosition.z);
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
		//Schema.SpriteID dirs = (Schema.SpriteID)((int)Schema.SpriteID.u_dir0 + (int)pos.dir);
		//o.transform.FindChild("Direction").GetComponent<SpriteRenderer>().sprite = Schema.Sprite.GetA(dirs).s.sprite;
		Play.Attrs.Core show = ent.GetAttr<Play.Attrs.Core>();
		if (show == null)
			return;
		Sprite sprite = show.GetSprite().s.sprite;
		o.GetComponent<SpriteRenderer>().sprite = sprite;
	}

	public static Vector2[] GenerateUV(Schema.Floor.A[,] tiles) {
		Assert.AreEqual(tiles.GetLength(0), World.GRID_SIZE);
		Assert.AreEqual(tiles.GetLength(1), World.GRID_SIZE);
		int xSize = World.GRID_SIZE;
		int ySize = World.GRID_SIZE;
		int v = 0;
		Vector2[] uv = new Vector2[xSize * ySize * 4];
		for (int y = 0; y < ySize; y++) {
			for (int x = 0; x < xSize; x++) {
				Schema.Floor.A t = tiles[x, y];
				Assert.IsNotNull(t, string.Format("tile is null in {0},{1}", x, y));
				Sprite sp = t.s.sprite.s.sprite;
				UnityEngine.Rect rect = sp.textureRect;
				float width = sp.texture.width;
				float height = sp.texture.height;
				uv[v++] = new Vector2(rect.xMin / width, rect.yMin / height);
				uv[v++] = new Vector2(rect.xMin / width, rect.yMax / height);
				uv[v++] = new Vector2(rect.xMax / width, rect.yMin / height);
				uv[v++] = new Vector2(rect.xMax / width, rect.yMax / height);
			}
		}
		return uv;
	}
}
