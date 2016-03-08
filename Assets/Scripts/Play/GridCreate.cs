//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Play {
	public abstract class LayerCreate {
		public abstract Grid.Data Create(Ctx ctx, Coord g);
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

		public override Grid.Data Create(Ctx ctx, Coord g) {
			Schema.Entity.A exit = Schema.Entity.GetA(Schema.Entity.ID.Workshop_Mine);
			Grid.Data grid = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					grid.tiles[x, y] = floors.Get(ctx);
					Coord c = g.Add(x, y);
					ctx.dstc = c;
					Schema.Entity.A a = null;
					if (c == ctx.layer.param.exit) {
						a = exit;
					} else {
						a = entities.Get(ctx);
					}
					if (a != null) {
						Entity e = a.CreateEntity(ctx);
						if (e != null) {
							grid.entities.Add(e);
						}
					}
				}
			}
			return grid;
		}
	}

	public class Cave : LayerCreate {
		public override Grid.Data Create(Ctx ctx, Coord g) {
			Schema.Entity.A border = Schema.Entity.GetA(Schema.Entity.ID.Workshop_Campfire);
			Schema.Entity.A stone = Schema.Entity.GetA(Schema.Entity.ID.Boulder);
			Schema.Entity.A exit = Schema.Entity.GetA(Schema.Entity.ID.Workshop_Mine);
			Schema.Floor.A floor = Schema.Floor.GetA(Schema.Floor.ID.Dirt);
			int z = ctx.layer.z;
			int s = ctx.layer.world.param.seed;
			Rect rect = ctx.layer.param.rect;
			Grid.Data grid = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					grid.tiles[x, y] = floor;
					Coord c = g.Add(x, y);
					ctx.dstc = c;
					Schema.Entity.A a = null;
					if (c == ctx.layer.param.exit) {
						a = exit;
					} else if (!c.In(rect)) {
						a = border;
					} else {
						int room = Rand.BigCave(x + s, y + s, z + s);
						if (room > 0) {
							a = stone;
						}
					}
					if (a != null) {
						Entity e = stone.CreateEntity(ctx);
						if (e != null) {
							grid.entities.Add(e);
						}
					}
				}
			}
			return grid;
		}
	}
}
