using System;
using System.Collections.Generic;

namespace Schema {
	public sealed class Grid : SchemaBase<Grid.ID, Grid> {
		public readonly Dictionary<Floor.ID, int> floors;
		public readonly Dictionary<Entity.ID, int> entities;
		private Grid(Dictionary<Floor.ID, int> floors, Dictionary<Entity.ID, int> entities) {
			this.floors = floors;
			this.entities = entities;
		}
		public enum ID {
			Plain,
		}
		static public void Init () {
			Add(ID.Plain, new Grid(
				floors: new Dictionary<Floor.ID, int>() {
					{ Floor.ID.Dirt, 60 },
					{ Floor.ID.Grass, 40 },
				},
				entities: new Dictionary<Entity.ID, int>() {
					{ Entity.ID.Boulder, 10 },
					{ Entity.ID.Human, 10 },
					{ Entity.ID.Tree_Oak, 10 },
					{ Entity.ID.Tree_Pine, 10 },
					{ Entity.ID.None, 60 },
				}
			));
		}
	}
}
