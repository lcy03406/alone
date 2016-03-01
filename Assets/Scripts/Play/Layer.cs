//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class Layer {
		public const int GRID_SIZE = 32; //must be power of 2
		public const int GRID_MASK = GRID_SIZE - 1;
		const int LOAD_RADIUS = 30;
		const int SAVE_RADIUS = 60;
		const int UNLOAD_SIZE = 20; //

		public World world;
		public int id;

		SortedList<Coord, Grid> grids = new SortedList<Coord, Grid> ();
		SortedList<WUID, Entity> entities = new SortedList<WUID, Entity> ();

		public void Save () {
			foreach (KeyValuePair<Coord, Grid> pair in grids) {
				world.file.SaveGrid (id, pair.Key, pair.Value);
			}
		}

		Grid GetGrid (Coord g) {
			Grid grid = FindGrid (g);
			if (grid == null)
				grid = LoadGrid (g);
			return grid;
		}

		Grid FindGrid (Coord g) {
			Grid grid;
			if (grids.TryGetValue (g, out grid)) {
				return grid;
			}
			return null;
		}

		Grid LoadGrid (Coord g) {
			Grid grid = new Grid (this, g);
			grids.Add (g, grid);
			Grid.Data d = world.file.LoadGrid (id, g);
			if (d == null) {
				d = CreateGrid (g);
			}
			grid.Load(d);
			if (world.view != null && world.param.layer == id) {
				world.view.OnLoadGrid (g, grid);
			}
			return grid;
		}

		Grid.Data CreateGrid (Coord g) {
			Ctx ctx = new Ctx(this, null, null);
			GridCreate cre = Schema.Grid.GetA(Schema.Grid.ID.Plain).s.cre;
			Grid.Data grid = cre.Create(ctx, g);
			return grid;
		}

		void Anchor (Coord anchor) {
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
				foreach (KeyValuePair<Coord, Grid> pair in grids) {
					if (!pair.Key.In (saver)) {
						Coord g = pair.Key;
						Grid grid = pair.Value;
						world.file.SaveGrid (id, g, grid);
						grid.Unload ();
						if (world.view != null && world.param.layer == id) {
							world.view.OnUnloadGrid (g);
						}
						del.Add (g);
					}
				}
				foreach (Coord g in del) {
					grids.Remove (g); ;
				}
			}
		}

		void UnloadAllGrids() {
			List<Coord> del = new List<Coord>();
			foreach (KeyValuePair<Coord, Grid> pair in grids) {
				Coord g = pair.Key;
				Grid grid = pair.Value;
				world.file.SaveGrid(id, g, grid);
				grid.Unload();
				if (world.view != null && world.param.layer == id) {
					world.view.OnUnloadGrid(g);
				}
				del.Add(g);
			}
			foreach (Coord g in del) {
				grids.Remove(g); ;
			}
		}

		public Entity FindEntity (WUID id) {
			Entity ent;
			if (entities.TryGetValue (id, out ent)) {
				return ent;
			}
			return null;
		}

		public void AddEntity (Entity ent) {
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			if (ent.isPlayer) {
				Anchor(pos.c);
			} else {
				entities.Add(ent.id, ent);
				Coord to = pos.c.Grid();
				Grid tg = FindGrid(to);
				tg.MoveIn(ent);
			}
			ent.layer = this;
			if (world.view != null && world.param.layer == id) {
				world.view.OnAddEntity (ent);
			}
		}

		public void DelEntity (Entity ent) {
			if (world.view != null && world.param.layer == id) {
				world.view.OnDelEntity (ent);
			}
			Assert.AreEqual (ent.layer, this);
			if (ent.isPlayer) {
				UnloadAllGrids();
			} else {
				Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
				Coord from = pos.c.Grid();
				Grid fg = FindGrid(from);
				fg.MoveOut(ent);
				entities.Remove(ent.id);
			}
			ent.layer = null;
		}

		public void MoveOut(Entity ent) {
			AddEntity(ent);
		}

		public void MoveIn(Entity ent) {
			DelEntity(ent);
		}

		public void MoveEntity (Entity ent, Coord to) {
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			Coord from = pos.c;
			pos.c = to;
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

		public void Tick (int time) {
			int i = 0;
			while (i < entities.Count) {
				WUID id = entities.Keys[i];
				Entity ent = entities.Values[i];
				ent.Tick (time);
				while (i < entities.Count && entities.Keys[i] < id) {
					i++;
				}
				i++;
			}
			foreach (KeyValuePair<Coord, Grid> pair in grids) {
				pair.Value.Update (time);
			}
		}

		public void MoveCrossGrid (Entity entity, Coord from, Coord to) {
			Grid fg = FindGrid (from);
			Grid tg = FindGrid (to);
			fg.MoveOut (entity);
			tg.MoveIn (entity);
		}

		public bool CanMoveTo (Coord to) {
			Grid tg = FindGrid (to.Grid ());
			if (tg == null)
				return false;
			return tg.FindEntity (to) == null;
		}
		public Entity SearchEntity (Coord to) {
			Grid tg = FindGrid (to.Grid ());
			if (tg == null)
				return null;
			return tg.FindEntity (to);
		}
	}
}
