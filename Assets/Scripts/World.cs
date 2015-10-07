using System;
using System.Collections.Generic;
using System.Diagnostics;

public class World {
	public interface View {
		void OnLoadGrid (Coord g, WorldGrid grid);
		void OnUnloadGrid (Coord g);
		void OnLoadPlayer (WorldEntity player);
		void OnAddEntity (WorldEntity ent);
		void OnDelEntity (WorldEntity ent);
	}
	public const int GRID_SIZE = 32; //must be power of 2
	public const int GRID_MASK = GRID_SIZE - 1;
	const int LOAD_RADIUS = 30;
	const int SAVE_RADIUS = 60;
	const int UNLOAD_SIZE = 20; //
	public static World singleton;
	
	public Scheme scheme;
	public View view;
	WorldFile file;
	Random rand;
	SortedList<Coord, WorldGrid> grids = new SortedList<Coord, WorldGrid>();
	SortedList<WUID, WorldEntity> entities = new SortedList<WUID, WorldEntity>();
	WorldEntity player;
	[Serializable]
	public class Param {
		public WUID maxid;
		public int time;
	};
	public Param param;

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
		rand = new Random ();
		grids.Clear ();
		file = new WorldFile ();
		file.LoadWorld (path, name);
		param = file.LoadParam ();
		if (param == null) {
			param = new Param ();
		}
		WorldEntity.Data e = file.LoadPlayer ();
		if (e == null) {
			e = WorldEntity.Create (this, Scheme.Creature.ID.Human);
			e.ai = null;
		}
		player = new WorldEntity (this, e);
		view.OnLoadPlayer (player);
		Anchor (player.d.c);
	}

	public void SaveWorld () {
		file.SaveParam (param);
		file.SavePlayer (player);
		foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
			file.SaveGrid (pair.Key, pair.Value);
		}
		file.SaveWorld ();
	}

	WorldGrid GetGrid (Coord g) {
		WorldGrid grid = FindGrid (g);
		if (grid == null)
			grid = LoadGrid (g);
		return grid;
	}

	WorldGrid FindGrid (Coord g) {
		WorldGrid grid;
		if (grids.TryGetValue (g, out grid)) {
			return grid;
		}
		return null;
	}

	WorldGrid LoadGrid (Coord g) {
		WorldGrid.Data d = file.LoadGrid (g);
		if (d == null) {
			d = CreateGrid (g);
		}
		WorldGrid grid = new WorldGrid (this, g, d);
		grids.Add (g, grid);
		if (view != null) {
			view.OnLoadGrid (g, grid);
		}
		return grid;
	}

	WorldGrid.Data CreateGrid (Coord g) {
		WorldGrid.Data grid = new WorldGrid.Data ();
		for (int x = 0; x < GRID_SIZE; ++x) {
			for (int y = 0; y < GRID_SIZE; ++y) {
				grid.tiles [x,y] = rand.Next (1, (int)Scheme.Floor.ID.Size);
				if (rand.Next (0, 100) < 10) {
					WorldEntity.Data e = WorldEntity.Create (this, Scheme.Creature.ID.Human);
					e.c = g.Add (x, y);
					e.dir = 0;
					grid.entities.Add (e);
				}
			}
		}
		return grid;
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
					file.SaveGrid (g, grid);
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

	public WUID NextWUID () {
		param.maxid = param.maxid.Next ();
		return param.maxid;
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
		Debug.Assert (ent.world == this);
		ent.world = null;
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
	}
	
	public void Update () {
		while (param.time < player.d.actime) {
			param.time ++;
			player.Update(param.time);
			int i = 0;
			WUID id = new WUID();
			while (i < entities.Count) {
				id = entities.Keys[i];
				WorldEntity ent = entities.Values[i];
				ent.Update (param.time);
				while (i < entities.Count && entities.Keys[i] < id) {
					i++;
				}
				i++;
			}
			foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
				pair.Value.Update (param.time);
			}
		}

	}

	public void MoveCrossGrid (WorldEntity entity, Coord from, Coord to) {
		WorldGrid fg = FindGrid (from);
		WorldGrid tg = FindGrid (to);
		fg.MoveOut (entity);
		tg.MoveIn (entity);
	}

	public bool CanMoveTo (Coord to) {
		WorldGrid tg = FindGrid (to.Grid());
		if (tg == null)
			return false;
		return tg.FindEntity (to) == null;
	}
	public WorldEntity SearchEntity (Coord to) {
		WorldGrid tg = FindGrid (to.Grid());
		if (tg == null)
			return null;
		return tg.FindEntity (to);
	}
}

