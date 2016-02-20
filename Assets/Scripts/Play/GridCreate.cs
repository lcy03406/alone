//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	public class GridCreate {
		Calcs.RandConst<Schema.Floor.A> floors;
		Calcs.RandConst<EntityCreate> entities;
		public GridCreate(Dictionary<Schema.Floor.ID, int> floors,
			Dictionary<Schema.Entity.ID, int> entities)
		{
			Dictionary<Schema.Floor.A, int> flos = new Dictionary<Schema.Floor.A, int>();
			foreach (KeyValuePair<Schema.Floor.ID, int> pair in floors) {
				flos.Add(Schema.Floor.GetA(pair.Key), pair.Value);
			}
            this.floors = new Calcs.RandConst<Schema.Floor.A>(flos);
			Dictionary<EntityCreate, int> ents = new Dictionary<EntityCreate, int>();
			foreach (KeyValuePair<Schema.Entity.ID, int> pairs in entities) {
				ents.Add(new EntityCreate(Schema.Entity.GetA(pairs.Key)), pairs.Value);
            }
			this.entities = new Calcs.RandConst<EntityCreate>(ents);
		}

		public Grid.Data Create(Ctx ctx, Coord g) {
			Grid.Data grid = new Grid.Data();
			for (int x = 0; x < World.GRID_SIZE; ++x) {
				for (int y = 0; y < World.GRID_SIZE; ++y) {
					grid.tiles[x, y] = floors.Get(ctx);
					Entity e = entities.Get(ctx).Create(ctx);
					if (e != null) {
						e.c = g.Add(x, y);
						grid.entities.Add(e);
					}
				}
			}
			return grid;
		}
	}
}
