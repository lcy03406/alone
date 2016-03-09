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

		[Serializable]
		public class Param {
			public Rect rect;
			public Coord entr;
			public Coord exit;
		};
		public Param param;

		public World world;
		public int z;

		SortedList<Coord, Grid> grids = new SortedList<Coord, Grid> ();
		SortedList<WUID, Entity> entities = new SortedList<WUID, Entity>();
		SortedList<int, List<Entity>> tick_ents = new SortedList<int, List<Entity>>();

		public Layer(World world, int z) {
			this.world = world;
			this.z = z;
		}

		public void Load() {
			param = world.file.LoadLayerParam(z);
			if (param == null) {
				Init();
			}
		}

		public void Init() {
			param = new Param();
			//TODO
			if (z < 0) {
				int r = 10 + z;
				param.rect = new Rect(new Coord(-r, -r), new Coord(r - 1, r - 1));
			}
			param.entr = new Coord(2, 1);
			param.exit = new Coord(1, 1);
		}

		public void Save () {
			world.file.SaveLayerParam(z, param);
			foreach (KeyValuePair<Coord, Grid> pair in grids) {
				world.file.SaveGrid (z, pair.Key, pair.Value);
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
			Grid.Data d = world.file.LoadGrid (z, g);
			if (d == null) {
				d = CreateGrid (g);
			}
			grid.Load(d);
			if (world.view != null && world.param.layer == z) {
				world.view.OnGridLoad (g, grid);
			}
			return grid;
		}

		Grid.Data CreateGrid (Coord g) {
			Ctx ctx = new Ctx(this, g);
			Schema.Grid.ID id = z >= 0 ? Schema.Grid.ID.Plain : Schema.Grid.ID.Cave;
			LayerCreate cre = Schema.Grid.GetA(id).s.cre;
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
						if (world.view != null && world.param.layer == z) {
							world.view.OnGridUnload (g);
						}
						world.file.SaveGrid (z, g, grid);
						grid.Unload ();
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
				if (world.view != null && world.param.layer == z) {
					world.view.OnGridUnload(g);
				}
				world.file.SaveGrid(z, g, grid);
				grid.Unload();
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
				int next_tick = ent.NextTick();
				if (next_tick < int.MaxValue) {
					AddTick(next_tick, ent);
				}
				Coord to = pos.c.Grid();
				Grid tg = FindGrid(to);
				tg.MoveIn(ent);
			}
			ent.layer = this;
			if (world.view != null && world.param.layer == z) {
				world.view.OnEntityAdd (ent);
			}
		}

		public void DelEntity (Entity ent) {
			if (world.view != null && world.param.layer == z) {
				world.view.OnEntityDel (ent);
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

		public void MoveIn(Entity ent) {
			AddEntity(ent);
		}

		public void MoveOut(Entity ent) {
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

		public void AddTick(int time, Entity ent) {
			List<Entity> list = null;
			if (!tick_ents.TryGetValue(time, out list)) {
				list = new List<Entity>();
				tick_ents.Add(time, list);
			}
			list.Add(ent);
		}

		public void Tick (int time) {
			while (tick_ents.Count > 0) {
				int tick = tick_ents.Keys[0];
				if (tick > time)
					break;
				List<Entity> ents = tick_ents.Values[0];
				int i = 0;
				while (i < ents.Count) {
					Entity ent = ents[i];
					WUID id = ent.id;
					if (entities.ContainsKey(id)) {
						ent.Tick(time);
						if (entities.ContainsKey(id)) {
							if (world.view != null && world.param.layer == z) {
								world.view.OnEntityUpdate(ent);
							}
							int next_tick = ent.NextTick();
							AddTick(next_tick, ent);
						}
					}
					while (i < ents.Count && ents[i].id < id) {
						i++;
					}
					i++;
				}
				tick_ents.Remove(tick);
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
			Entity ent = tg.FindEntity (to);
			AddTick(world.param.time, ent);
			return ent;
		}
	}
}
