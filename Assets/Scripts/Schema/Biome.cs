using System;
using System.Collections.Generic;

namespace Schema {
	public sealed class Biome : SchemaBase<BiomeID, Biome> {
		public readonly Play.LayerCreate cre;
		private Biome(Play.LayerCreate cre) {
			this.cre = cre;
		}

		public static void AddAll(List<EditBiomeCave> edits) {
			foreach (EditBiomeCave edit in edits) {
				Add(edit.id, new Biome(cre: new Play.Layers.Cave(edit)));
			}
		}
		/*
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
							value: Entity.GetA(EntityID.Boulder),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(EntityID.Human),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(EntityID.Tree_Oak),
							prob: 10
						),
						new Play.Choice<Entity.A>(
							value: Entity.GetA(EntityID.Tree_Pine),
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
		*/
	}
}
