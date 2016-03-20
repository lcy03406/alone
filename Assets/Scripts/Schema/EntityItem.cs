//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void InitItem() {
			Stages item_stages = new Stages {
				{
					Stage.ID.Item_Static,
					new EntityStage(
						iact_src: null,
						iact_dst: new Iact.A[] {
							Iact.GetA (ActionID.PickItem),
						},
						iact_auto: Iact.GetA(ActionID.PickItem),
						make: null,
						usage: null
					)
				}
			};
			Add(EntityID.Item, new Entity(
				sprite: SpriteID.block_crate01,
				name: null,
				stages: item_stages,
				start_stage: Stage.GetA(Stage.ID.Item_Static),
				stat: null,
				part: new Play.Attrs.Part(),
				attr: new Play.Ents.Item() 
			));
		}
	}
}
