//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void AddAll(List<EditEntityWorkshop> edits) {
			Stages campfire_stages = new Stages {
				{
					Stage.ID.Workshop_Off,
					new EntityStage(
						iact_src: null,
						iact_dst: null,
						make: null,
						usage: null
					)
				},
				{
					Stage.ID.Workshop_On,
					new EntityStage(
						iact_src: null,
						iact_dst: null,
						make: null,
						usage: new Play.Attrs.Stat() {
						/*	ints = {
								{ StatID.Workshop_Cookfire, new Play.Attrs.Stat.St(
									value: 1,
									cap: 0
								)},
							}*/
						}
					)
				},
			};
			foreach (EditEntityWorkshop edit in edits) {
				List<Iact.A> iact_dst = new List<Iact.A>();
				foreach (ActionID act in edit.actions) {
					iact_dst.Add(Iact.GetA(act));
				}
				Stages stages = new Stages {
					{
						Stage.ID.Workshop_On,
						new EntityStage(
							iact_src: null,
							iact_dst: iact_dst.ToArray(),
							make: null,
							usage: null
						)
					},
				};
				Add(edit.id, new Entity(
					sprite: edit.sprite,
					name: edit.name,
					stages: stages,
					start_stage: Stage.GetA(Stage.ID.Workshop_On),
					stat: null,
					part: null,
					attr: null
				));
			}
		}
	}
}
