//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Play {
	public abstract class LayerCreate {
		public abstract void Create(Ctx ctx, Grid grid);
	}
}

namespace Play.Layers {
	public class Flat : LayerCreate {
		Calcs.RandConst<Schema.Floor.A> floors;
		Calcs.RandConst<Schema.Entity.A> entities;
		public Flat(Calcs.RandConst<Schema.Floor.A> floors,
			Calcs.RandConst<Schema.Entity.A> entities) {
			this.floors = floors;
			this.entities = entities;
		}

		public override void Create(Ctx ctx, Grid grid) {
			Coord g = grid.c;
			Schema.Entity.A exit = Schema.Entity.GetA(Schema.EntityID.WorkshopExit);
			Grid.Data d = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					d.tiles[x, y] = floors.Get(ctx);
				}
			}
			grid.d = d;
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					Coord c = g.Add(x, y);
					ctx.dstc = c;
					Schema.Entity.A a = null;
					if (c == ctx.layer.param.exit) {
						a = exit;
					} else {
						a = entities.Get(ctx);
					}
					if (a != null) {
						a.CreateEntity(ctx);
					}
				}
			}
		}
	}

	public class Cave : LayerCreate {
		private Schema.Floor.A floor;
		private Schema.Entity.A entr;
		private Schema.Entity.A exit;
		private Schema.Entity.A block;
		private Schema.Entity.A border;

		public Cave(Schema.EditBiomeCave edit) {
			this.floor = Schema.Floor.GetA(edit.floor);
			this.entr = Schema.Entity.GetA(edit.entr);
			this.exit = Schema.Entity.GetA(edit.exit);
			this.block = Schema.Entity.GetA(edit.block);
			this.border = Schema.Entity.GetA(edit.border);
		}
		public override void Create(Ctx ctx, Grid grid) {
			Stopwatch watch = new Stopwatch();
			watch.Start();
			Coord g = grid.c;
			int z = ctx.layer.z;
			int s = ctx.layer.world.param.seed;
			Rect rect = ctx.layer.param.rect;
			Grid.Data d = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					d.tiles[x, y] = floor;
				}
			}
			grid.d = d;
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					Coord c = g.Add(x, y);
					ctx.dstc = c;
					Schema.Entity.A aent = null;
					if (c == ctx.layer.param.exit) {
						aent = exit;
					} else if (c.On(rect)) {
						aent = border;
					} else if (!c.In(rect)) {
						aent = null;
					} else {
						int room = Rand.BigCave(x + s, y + s, z + s);
						if (room > 0) {
							aent = block; //TODO
						}
					}
					if (aent != null) {
						aent.CreateEntity(ctx);
					}
				}
			}
			watch.Stop();
			Debug.Log(string.Format("create grid time {0}", watch.Elapsed));
		}
	}
}
