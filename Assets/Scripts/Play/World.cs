//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class World {
		public interface View {
			void OnLoadGrid (Coord g, WorldGrid grid);
			void OnUnloadGrid (Coord g);
			void OnLoadPlayer (Entity player);
			void OnAddEntity (Entity ent);
			void OnDelEntity (Entity ent);
		}
		public const int GRID_SIZE = 32; //must be power of 2
		public const int GRID_MASK = GRID_SIZE - 1;
		const int LOAD_RADIUS = 30;
		const int SAVE_RADIUS = 60;
		const int UNLOAD_SIZE = 20; //
		public static World singleton;

		public View view;
		WorldFile file;
		Random rand;
		SortedList<Coord, WorldGrid> grids = new SortedList<Coord, WorldGrid> ();
		SortedList<WUID, Entity> entities = new SortedList<WUID, Entity> ();
		Entity player;
		[Serializable]
		public class Param {
			public WUID maxid;
			public int time;
		};
		public Param param;

		public World () {
			singleton = this;
		}

		public void SetView (View view) {
			this.view = view;
		}

		public void LoadWorld (string path, string name) {
			Schema.All.Init ();
			rand = new Random ();
			grids.Clear ();
			file = new WorldFile ();
			file.LoadWorld (path, name);
			param = file.LoadParam ();
			if (param == null) {
				param = new Param ();
			}
			Entity e = file.LoadPlayer ();
			if (e == null) {
				Schema.EntityCreate.A human = Schema.EntityCreate.GetA (Schema.EntityCreate.ID.Human);
				e = human.s.cre.Create(new Ctx(this, null, null));
				e.SetAttr (new Attrs.Ctrl ());
			}
			e.SetWorld (this);
			e.isPlayer = true;
			player = e;
			view.OnLoadPlayer (player);
			Anchor (player.c);
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
			Schema.EntityCreate.A tree = Schema.EntityCreate.GetA (Schema.EntityCreate.ID.Tree_Pine);
			Schema.EntityCreate.A human = Schema.EntityCreate.GetA (Schema.EntityCreate.ID.Human);
			Schema.Floor.A[] floors = {
				Schema.Floor.GetA (Schema.Floor.ID.Dirt),
				Schema.Floor.GetA (Schema.Floor.ID.Grass),
			};
			Ctx ctx = new Ctx(this, null, null);
            for (int x = 0; x < GRID_SIZE; ++x) {
				for (int y = 0; y < GRID_SIZE; ++y) {
					grid.tiles[x, y] = floors[rand.Next (0, floors.Length)];
					int rr = rand.Next (0, 100);
					if (rr < 10) {
						Entity e = human.s.cre.Create(ctx);
						e.c = g.Add (x, y);
						grid.entities.Add (e);
					} else if (rr < 30) {
						Entity e = tree.s.cre.Create(ctx);
						e.c = g.Add (x, y);
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
					Coord g = new Coord (gx, gy);
					if (FindGrid (g) == null) {
						LoadGrid (g);
					}
				}
			}
			if (grids.Count > UNLOAD_SIZE) {
				Rect saver = anchor.Area (SAVE_RADIUS).Grid ();
				List<Coord> del = new List<Coord> ();
				foreach (KeyValuePair<Coord, WorldGrid> pair in grids) {
					if (!pair.Key.In (saver)) {
						Coord g = pair.Key;
						WorldGrid grid = pair.Value;
						file.SaveGrid (g, grid);
						grid.Unload ();
						if (view != null) {
							view.OnUnloadGrid (g);
						}
						del.Add (g);
					}
				}
				foreach (Coord g in del) {
					grids.Remove (g); ;
				}

			}
		}

		public WUID NextWUID () {
			param.maxid = param.maxid.Next ();
			return param.maxid;
		}

		public Entity FindEntity (WUID id) {
			Entity ent;
			if (entities.TryGetValue (id, out ent)) {
				return ent;
			}
			return null;
		}

		public void AddEntity (Entity ent) {
			entities.Add (ent.id, ent);
			if (view != null) {
				view.OnAddEntity (ent);
			}
		}

		public void DelEntity (Entity ent) {
			if (view != null) {
				view.OnDelEntity (ent);
			}
			Assert.AreEqual (ent.world, this);
			ent.world = null;
			entities.Remove (ent.id);
		}

		public void MoveEntity (Entity ent, Coord to) {
			Coord from = ent.c;
			ent.c = to;
			Assert.AreNotEqual (from, to, ent.isPlayer.ToString ());
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
			player.Tick (param.time);
			while (param.time < player.NextTick ()) {
				param.time++;
				player.Tick (param.time);
				int i = 0;
				while (i < entities.Count) {
					WUID id = entities.Keys[i];
					Entity ent = entities.Values[i];
					ent.Tick (param.time);
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

		public void MoveCrossGrid (Entity entity, Coord from, Coord to) {
			WorldGrid fg = FindGrid (from);
			WorldGrid tg = FindGrid (to);
			fg.MoveOut (entity);
			tg.MoveIn (entity);
		}

		public bool CanMoveTo (Coord to) {
			WorldGrid tg = FindGrid (to.Grid ());
			if (tg == null)
				return false;
			return tg.FindEntity (to) == null;
		}
		public Entity SearchEntity (Coord to) {
			WorldGrid tg = FindGrid (to.Grid ());
			if (tg == null)
				return null;
			return tg.FindEntity (to);
		}
	}
}