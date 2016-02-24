//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	public class GridCreate {
		Calcs.RandConst<Schema.Floor.A> floors;
		Calcs.RandConst<EntityCreate> entities;
		public GridCreate(Calcs.RandConst<Schema.Floor.A> floors,
			Calcs.RandConst<EntityCreate> entities)
		{
            this.floors = floors;
			this.entities = entities;
		}

		public Grid.Data Create(Ctx ctx, Coord g) {
			Grid.Data grid = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					grid.tiles[x, y] = floors.Get(ctx);
					EntityCreate cre = entities.Get(ctx);
					if (cre != null) {
						Entity e = cre.Create(ctx);
						if (e != null) {
							e.c = g.Add(x, y);
							grid.entities.Add(e);
						}
					}
				}
			}
			return grid;
		}
	}
}
