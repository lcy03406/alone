//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void AddAll(List<EditEntityTree> edits) {
			Iact.A[] tree_iact = new Iact.A[] {
				Iact.GetA (ActionID.PickTreeBranch),
				Iact.GetA (ActionID.PickTreeFruit),
			};
			Stages tree_stages = new Stages {
				{
					Stage.ID.Tree_Young,
					new EntityStage(
						iact_src: null,
						iact_dst: tree_iact,
						make: null,
						usage: null
					)
				},
				{
					Stage.ID.Tree_Grown,
					new EntityStage(
						iact_src: null,
						iact_dst: tree_iact,
						make: null,
						usage: null
					)
				}
			};
			foreach (EditEntityTree edit in edits) {
				Add(edit.id, new Entity(
					sprite: edit.sprite,
					name: edit.name,
					stages: tree_stages,
					start_stage: Stage.GetA(Stage.ID.Tree_Young),
					stat: new Play.Attrs.Stat() {
						ints = {
						{ StatID.Grouth, new Play.Attrs.Stat.St(
							value: 0,
							cap: 100
						)},
						},
					},
					part: new Play.Attrs.Part() {
						items = {
						{ PartID.Tree_Branch, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.Branch),
							count: 10,
							cap: 10,
							q: 10,
							grow_span: 100,
							grow_count: 1
						)},
						{ PartID.Tree_Fruit, new Play.Attrs.Part.PartItem(
							a: Item.GetA(edit.fruit),
							count: 0,
							cap: edit.fruitCount,
							q: 10,
							grow_span: 1000,
							grow_count: edit.fruitCount
						)},
						}
					},
					attr: null
				));
			}
		}
	}
}
