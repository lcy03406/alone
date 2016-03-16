//utf-8。
using System;
using System.Collections.Generic;


namespace Schema {
	using Stages = SortedList<Stage.ID, EntityStage>;
	public sealed partial class Entity : SchemaBase<EntityID, Entity> {
		public static void AddAll(List<EditEntityCreature> edits) {
			Iact.A[] human_make = new Iact.A[] {
				Iact.GetA(ActionID.BuildCampfire),
				Iact.GetA(ActionID.MakeStoneKnife),
				Iact.GetA(ActionID.MakeStoneAxe),
				Iact.GetA(ActionID.MakeBoneKnife),
			};
			Iact.A[] human_iact_src = new Iact.A[] {
				Iact.GetA(ActionID.Dir),
				Iact.GetA(ActionID.Move),
				Iact.GetA(ActionID.AttackPunch),
				Iact.GetA(ActionID.BuildCampfire),
				Iact.GetA(ActionID.MakeStoneKnife),
				Iact.GetA(ActionID.MakeStoneAxe),
				Iact.GetA(ActionID.MakeBoneKnife),
				Iact.GetA(ActionID.PickBoulderStone),
				Iact.GetA(ActionID.PickTreeBranch),
				Iact.GetA(ActionID.PickTreeFruit),
				Iact.GetA(ActionID.PickCreatureMeat),
				Iact.GetA(ActionID.PickCreatureBone),
				Iact.GetA(ActionID.Rest),
				Iact.GetA(ActionID.TravelDown),
			};
			Iact.A[] creature_alive_iact_dst = new Iact.A[] {
				Iact.GetA(ActionID.AttackPunch),
			};
			Iact.A[] creature_dead_iact_dst = new Iact.A[] {
				Iact.GetA(ActionID.PickCreatureMeat),
				Iact.GetA(ActionID.PickCreatureBone),
			};
			Stages human_stages = new Stages {
				{
					Stage.ID.Creature_Alive,
					new EntityStage(
						iact_src: human_iact_src,
						iact_dst: creature_alive_iact_dst,
						make: human_make,
						usage: null
					)
				},
				{
					Stage.ID.Creature_Dead,
					new EntityStage(
						iact_src: null,
						iact_dst: creature_dead_iact_dst,
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
