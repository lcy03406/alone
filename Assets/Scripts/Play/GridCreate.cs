//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Play {
	public class GridCreate {
		Calcs.RandConst<Schema.Floor.A> floors;
		Calcs.RandConst<Schema.Entity.A> entities;
		public GridCreate(Calcs.RandConst<Schema.Floor.A> floors,
			Calcs.RandConst<Schema.Entity.A> entities)
		{
            this.floors = floors;
			this.entities = entities;
		}

		public Grid.Data Create(Ctx ctx, Coord g) {
			Grid.Data grid = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					//grid.tiles[x, y] = floors.Get(ctx);
					float per = Mathf.PerlinNoise((float)x / (float)World.GRID_SIZE * 2, (float)y / (float)World.GRID_SIZE) * 2;
					if (per >= 0.3)
						grid.tiles[x, y] = Schema.Floor.GetA(Schema.Floor.ID.Grass);
					else
						grid.tiles[x, y] = Schema.Floor.GetA(Schema.Floor.ID.Ocean);

					Schema.Entity.A cre = entities.Get(ctx);
					if (cre != null) {
						Entity e = cre.CreateEntity(ctx);
						if (e != null) {
							e.GetAttr<Attrs.Pos>().c = g.Add(x, y);
							grid.entities.Add(e);
						}
					}
				}
			}
			return grid;
		}
	}
}
