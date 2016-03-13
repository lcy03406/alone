//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void AddAll(List<EditEntityCreature> edits) {
			Iact.A[] human_make = new Iact.A[] {
				Iact.GetA(ActionID.Make_Cross),
				Iact.GetA(ActionID.Make_Knife_Stone),
				Iact.GetA(ActionID.Make_Axe_Stone),
				Iact.GetA(ActionID.Make_Knife_Bone),
				Iact.GetA(ActionID.Build_Campfire),
			};
			Iact.A[] human_iact_src = new Iact.A[] {
				Iact.GetA(ActionID.Rest),
				Iact.GetA(ActionID.Attack_Punch),
				Iact.GetA(ActionID.Travel_Down),
				Iact.GetA(ActionID.Chip_Stone),
				Iact.GetA(ActionID.Tree_PickBranch),
				Iact.GetA(ActionID.Tree_PickFruit),
				Iact.GetA(ActionID.Butcher_Meat),
				Iact.GetA(ActionID.Butcher_Bone),
				Iact.GetA(ActionID.Make_Cross),
				Iact.GetA(ActionID.Make_Knife_Stone),
				Iact.GetA(ActionID.Make_Axe_Stone),
				Iact.GetA(ActionID.Make_Knife_Bone),
				Iact.GetA(ActionID.Build_Campfire),
			};
			Iact.A[] creature_iact_dst = new Iact.A[] {
				Iact.GetA(ActionID.Butcher_Meat),
				Iact.GetA(ActionID.Butcher_Bone),
			};
			Stages human_stages = new Stages {
				{
					Stage.ID.Creature_Alive,
					new EntityStage(
						iact_src: human_iact_src,
						iact_dst: null,
						make: human_make,
						usage: null
					)
				},
				{
					Stage.ID.Creature_Dead,
					new EntityStage(
						iact_src: null,
						iact_dst: creature_iact_dst,
						make: null,
						usage: null
					)
				}
			};
			foreach (EditEntityCreature edit in edits) {
				Add(edit.id, new Entity(
					sprite: edit.sprite,
					name: edit.name,
					stages: human_stages,
					start_stage: Stage.GetA(Stage.ID.Creature_Alive),
					stat: new Play.Attrs.Stat() {
						ints = {
							{ StatID.HitPoint, new Play.Attrs.Stat.St(
								value: edit.hp,
								cap: edit.hp
							)},
							{ StatID.Stamina, new Play.Attrs.Stat.St(
								value: edit.stamina,
								cap: edit.stamina
							)},
							{ StatID.Attack, new Play.Attrs.Stat.St(
								value: edit.attack,
								cap: 0
							)},
							{ StatID.Defence, new Play.Attrs.Stat.St(
								value: edit.defence,
								cap: 0
							)},
						},
					},
					part: new Play.Attrs.Part() {
						items = {
						{ PartID.Creature_Bone, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.Bone),
							count: edit.stamina,
							cap: edit.stamina,
							q: 10,
							grow_span: 0,
							grow_count: 0
						)},
						{ PartID.Creature_Meat, new Play.Attrs.Part.PartItem(
							a: Item.GetA(ItemID.Meat),
							count: edit.hp,
							cap: edit.hp,
							q: 10,
							grow_span: 0,
							grow_count: 0
						)},
						}
					},
					attr: new Play.Ents.Creature(
					)
				));
			}
		}
	}
}
