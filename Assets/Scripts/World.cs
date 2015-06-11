using System;
using System.IO;
using System.Collections.Generic;
using Formater = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;


public class World {
	public interface View {
		void OnLoadGrid (Coord g, WorldGrid grid);
		void OnUnloadGrid (Coord g);
		void OnLoadPlayer (WorldEntity player);
		void OnAddEntity (WorldEntity ent);
		void OnDelEntity (WorldEntity ent);
		void OnMoveEntity (WorldEntity ent);
	}
	public const int GRID_SIZE = 32; //must be power of 2
	public const int GRID_MASK = GRID_SIZE - 1;
	const int LOAD_RADIUS = 30;
	const int SAVE_RADIUS = 60;
	const int UNLOAD_SIZE = 20; //
	public static World singleton;
	
	public Scheme scheme;
	public View view;
	string path;
	string name;
	string pathBackup;
	string pathCurrent;
	string pathName;
	Dictionary<Coord, WorldGrid> grids = new Dictionary<Coord, WorldGrid>();
	Dictionary<WUID, WorldEntity> entities = new Dictionary<WUID, WorldEntity>();
	WUID maxid;
	WorldEntity player;
	public int time;
	Random rand;

	public World ()
	{
		singleton = this;
	}

	public void SetView (View view) {
		this.view = view;
	}

	public void LoadWorld (string path, string name) {
		scheme = new Scheme ();
		scheme.LoadAll ();
		this.path = path;
		this.name = name;
		pathBackup = path + ".backup/";
		pathCurrent = path + ".current/";
		pathName = path + name + "/";
		grids.Clear ();
		rand = new Random ();
		LoadPlayer ();
		Anchor (player.d.c);
	}

	public void SaveWorld () {
		SavePlayer ();
		foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
			SaveGrid (pair.Key, pair.Value);
		}
		Directory.CreateDirectory (path + name);
		if (Directory.Exists (pathBackup)) {
			Directory.Delete (pathBackup, true);
		}
		Directory.CreateDirectory (pathBackup);
		string[] names = Directory.GetFiles (pathCurrent);
		foreach (string fullname in names) {
			string filename = Path.GetFileName (fullname);
			string namefile = pathName + filename;
			if (File.Exists (namefile)) {
				File.Move (namefile, pathBackup + filename);
			}
			File.Move (pathCurrent + filename, namefile);
		}
	}

	WorldGrid FindGrid (Coord g) {
		WorldGrid grid;
		if (grids.TryGetValue (g, out grid)) {
			return grid;
		}
		return null;
	}

	WorldGrid LoadGrid (Coord g) {

		WorldGrid grid;
		string filename = pathName + string.Format ("/grid_" + g + ".bin");
		if (File.Exists (filename)) {
			FileStream fs = File.Open (filename, FileMode.Open);
			Formater f = new Formater();
			WorldGrid.Data data = f.Deserialize (fs) as WorldGrid.Data;
			fs.Close ();
			grid = new WorldGrid (this, g, data);
		} else {
			grid = CreateGrid (g);
		}
		grids.Add (g, grid);
		if (view != null) {
			view.OnLoadGrid (g, grid);
		}
		return grid;
	}

	WorldGrid CreateGrid (Coord g) {
		WorldGrid.Data grid = new WorldGrid.Data ();
		for (int x = 0; x < GRID_SIZE; ++x) {
			for (int y = 0; y < GRID_SIZE; ++y) {
				grid.tiles [x,y] = rand.Next (1, (int)Scheme.Floor.ID.Size);
				if (rand.Next (0, 100) < 20) {
					WorldEntity.Data e = WorldEntity.Create (this, Scheme.Creature.ID.Human);
					e.c = g.Add (x, y);
					e.dir = 0;
					grid.entities.Add (e);
				}
			}
		}
		return new WorldGrid (this, g, grid);
	}
	
	void SaveGrid (Coord g, WorldGrid grid) {
		Directory.CreateDirectory (pathCurrent);
		string filename = pathCurrent + string.Format ("/grid_" + g + ".bin");
		FileStream fs = File.Open (filename, FileMode.Create);
		Formater f = new Formater ();
		WorldGrid.Data data = grid.Save ();
		f.Serialize (fs, data);
		fs.Close ();
	}

	public void Anchor (Coord anchor) {
		Rect loadr = anchor.Area (LOAD_RADIUS).Grid ();
		for (int gx = loadr.bl.x; gx <= loadr.tr.x; gx += GRID_SIZE) {
			for (int gy = loadr.bl.y; gy <= loadr.tr.y; gy += GRID_SIZE) {
				Coord g = new Coord(gx, gy);
				if (FindGrid (g) == null) {
					LoadGrid (g);
				}
			}
		}
		if (grids.Count > UNLOAD_SIZE) {
			Rect saver = anchor.Area (SAVE_RADIUS).Grid ();
			List<Coord> del = new List<Coord>();
			foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
				if (!pair.Key.In (saver)) {
					Coord g = pair.Key;
					WorldGrid grid = pair.Value;
					SaveGrid (g, grid);
					grid.Unload();
					if (view != null) {
						view.OnUnloadGrid (g);
					}
					del.Add(g);
				}
			}
			foreach (Coord g in del) {
				grids.Remove (g);;
			}
			
		}
	}

	void LoadPlayer () {
		string filename = pathName + string.Format ("/player.bin");
		if (File.Exists (filename)) {
			FileStream fs = File.Open (filename, FileMode.Open);
			Formater f = new Formater();
			time = (int) f.Deserialize (fs);
			maxid = (WUID) f.Deserialize (fs);
			WorldEntity.Data e = f.Deserialize (fs) as WorldEntity.Data;
			fs.Close ();
			player = new WorldEntity (this, e);
		} else {
			time = 0;
			maxid = (WUID) 0;
			WorldEntity.Data e = WorldEntity.Create (this, Scheme.Creature.ID.Human);
			player = new WorldEntity (this, e);
		}
		if (view != null) {
			view.OnLoadPlayer (player);
		}
	}

	void SavePlayer () {
		Directory.CreateDirectory (pathCurrent);
		string filename = pathCurrent + string.Format ("/player.bin");
		FileStream fs = File.Open (filename, FileMode.Create);
		Formater f = new Formater ();
		WorldEntity.Data data = player.Save ();
		f.Serialize (fs, time);
		f.Serialize (fs, maxid);
		f.Serialize (fs, data);
		fs.Close ();
	}

	public WUID NextWUID () {
		maxid = new WUID (maxid.value + 1);
		return maxid;
	}

	public WorldEntity FindEntity (WUID id) {
		WorldEntity ent;
		if (entities.TryGetValue (id, out ent)) {
			return ent;
		}
		return null;
	}

	public void AddEntity (WorldEntity ent) {
		entities.Add (ent.d.id, ent);
		if (view != null) {
			view.OnAddEntity (ent);
		}
	}

	public void DelEntity (WorldEntity ent) {
		if (view != null) {
			view.OnDelEntity (ent);
		}
		entities.Remove (ent.d.id);
	}

	public void MoveEntity (WorldEntity ent, Coord to) {
		Coord from = ent.d.c;
		ent.d.c = to;
		if (ent.isPlayer) {
			Anchor (to);
		} else {
			Coord fg = from.Grid ();
			Coord tg = to.Grid ();
			if (fg != tg) {
				MoveCrossGrid (ent, fg, tg);
			}
		}
		if (view != null) {
			view.OnMoveEntity (ent);
		}
	}
	
	public void Update () {
		while (time < player.d.actime) {
			time ++;
			player.Update(time);
			foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
				pair.Value.Update (time);
			}
		}

	}

	public void MoveCrossGrid (WorldEntity entity, Coord from, Coord to) {
		WorldGrid fg = FindGrid (from);
		WorldGrid tg = FindGrid (to);
		fg.MoveOut (entity);
		tg.MoveIn (entity);
	}

	public WorldEntity SearchEntity (Coord to) {
		WorldGrid tg = FindGrid (to.Grid());
		if (tg == null)
			return null;
		return tg.FindEntity (to);
	}
}

