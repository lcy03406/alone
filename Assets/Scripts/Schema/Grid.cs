using System;
using System.Collections.Generic;

namespace Schema {
	public sealed class Grid : SchemaBase<Grid.ID, Grid> {
		public readonly Play.LayerCreate cre;
		private Grid(Play.LayerCreate cre) {
			this.cre = cre;
		}
		public enum ID {
			Plain,
			Cave,
		}
		static public void Init () {
			Add(ID.Plain, new Grid(cre: new Play.Layers.Flat(
				floors: new Play.Calcs.RandConst<Floor.A>(
					choices: new List<Play.Choice<Floor.A>> {
						new Play.Choice<Floor.A>(
							value: Floor.GetA(Floor.ID.Dirt), 
							prob: 60
						),
						new Play.Choice<Floor.A>(
							value: Floor.GetA(Floor.ID.Grass),
							prob: 40
						),
					}
				),
				entities: new Play.Calcs.RandConst<Entity.A>(
					choices: new List<Play.Choice<Entity.A>> {
						new Play.Choice<Entity.A>(
							value: Entity.GetA(Entity.ID.Boulder),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(Entity.ID.Human),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(Entity.ID.Tree_Oak),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(Entity.ID.Tree_Pine),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: null,
							prob: 60
						),
					}
				)
			)));
			Add(ID.Cave, new Grid(cre: new Play.Layers.Cave()));
        }
	}
}
