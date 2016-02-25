//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class World {
		public interface View {
			void OnLoadGrid (Coord g, Grid grid);
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
		public Random rand;
		SortedList<Coord, Grid> grids = new SortedList<Coord, Grid> ();
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
				Ctx ctx = new Ctx(this, null, null);
                Schema.Entity.A human = Schema.Entity.GetA (Schema.Entity.ID.Human);
				e = Entity.Create(ctx, human);
				e.SetAttr (new Attrs.Ctrl ());
			}
			Attrs.Pos pos = e.GetAttr<Attrs.Pos>();
			e.world = this;
			e.isPlayer = true;
			player = e;
			view.OnLoadPlayer (player);
			Anchor (pos.c);
		}

		public void SaveWorld () {
			file.SaveParam (param);
			file.SavePlayer (player);
			foreach (KeyValuePair<Coord, Grid> pair in grids) {
				file.SaveGrid (pair.Key, pair.Value);
			}
			file.SaveWorld ();
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
			Grid.Data d = file.LoadGrid (g);
			if (d == null) {
				d = CreateGrid (g);
			}
			grid.Load(d);
			if (view != null) {
				view.OnLoadGrid (g, grid);
			}
			return grid;
		}

		Grid.Data CreateGrid (Coord g) {
			Ctx ctx = new Ctx(this, null, null);
			GridCreate cre = Schema.Grid.GetA(Schema.Grid.ID.Plain).s.cre;
			Grid.Data grid = cre.Create(ctx, g);
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
				foreach (KeyValuePair<Coord, Grid> pair in grids) {
					if (!pair.Key.In (saver)) {
						Coord g = pair.Key;
						Grid grid = pair.Value;
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
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			entities.Add (ent.id, ent);
			Coord to = pos.c.Grid();
			Grid tg = FindGrid(to);
			tg.MoveIn(ent);
			ent.world = this;
			if (view != null) {
				view.OnAddEntity (ent);
			}
		}

		public void DelEntity (Entity ent) {
			if (view != null) {
				view.OnDelEntity (ent);
			}
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			Assert.AreEqual (ent.world, this);
			ent.world = null;
			Coord from = pos.c.Grid();
			Grid fg = FindGrid(from);
			fg.MoveOut(ent);
			entities.Remove (ent.id);
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
				foreach (KeyValuePair<Coord, Grid> pair in grids) {
					pair.Value.Update (param.time);
				}
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
