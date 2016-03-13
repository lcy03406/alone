//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void AddAll(List<EditEntityBoulder> edits) {
			Stages boulder_stages = new Stages {
				{
					Stage.ID.Boulder_Static,
					new EntityStage(
						iact_src: null,
						iact_dst: new Iact.A[] {
							Iact.GetA (ActionID.Chip_Stone),
						},
						make: null,
						usage: null
					)
				}
			};
			foreach (EditEntityBoulder edit in edits) {
				Add(edit.id, new Entity(
					sprite: edit.sprite,
					name: edit.name,
					stages: boulder_stages,
					start_stage: Stage.GetA(Stage.ID.Boulder_Static),
					stat: new Play.Attrs.Stat() {
						ints = {
							{ StatID.HitPoint, new Play.Attrs.Stat.St(
								value: edit.hp,
								cap: edit.hp
							)},
							{ StatID.Defence, new Play.Attrs.Stat.St(
								value: edit.defence,
								cap: 0
							)},
						},
					},
					part: new Play.Attrs.Part() {
						items = {
							{ PartID.Boulder_Stone, new Play.Attrs.Part.PartItem(
								a: Item.GetA(edit.item),
								count: 1,
								cap: 1,
								q: 10,
								grow_span: 0,
								grow_count: 0
							)},
						}
					},
					attr: null
				));
			}
		}
	}
}
