//utf-8。
using System;
using System.Collections.Generic;

namespace Schema {
	public sealed partial class Iact : SchemaBase<ActionID, Iact> {
		public readonly string name;
		public readonly ActionCategoryID cat;
		public readonly int time1;
		public readonly int time2;
		public readonly bool has_dst;
		public readonly int distance;
		public readonly Play.Effect ef;
		public readonly int ani_state;
		public readonly int ani_once;

		public override string ToString() {
			return Play.Iact.Display(this);
		}

		private Iact(string name, ActionCategoryID cat, int time1, int time2,
				bool has_dst, int distance, Play.Effect ef,
				int ani_state, int ani_once) {
			this.name = name;
			this.cat = cat;
			this.time1 = time1;
			this.time2 = time2;
			this.has_dst = has_dst;
			this.distance = distance;
			this.ef = ef;
			this.ani_state = ani_state;
			this.ani_once = ani_once;
		}

		public static void AddAll(List<EditIactMove> edits) {
			foreach (EditIactMove edit in edits) {
				Play.Effect ef;
				if (edit.id == ActionID.Dir) {
					ef = Ef.Dir(edit.stamina);
				} else if (edit.id == ActionID.Move) {
					ef = Ef.Move(edit.stamina);
				} else {
					throw new ArgumentException("invalid moving action id");
				}
                Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Move,
					time1: edit.time1,
					time2: edit.time2,
					has_dst: false,
					distance: 0,
					ef: ef,
					ani_state: 2,
					ani_once: 0
				));
			}
		}

		public static void AddAll(List<EditIactAttack> edits) {
			foreach (EditIactAttack edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Attack,
					time1: edit.time1,
					time2: edit.time2,
					has_dst: true,
					distance: 1,
					ef: Ef.Attack(edit.stamina, edit.mulDamage, edit.addDamage),
					ani_state: 3,
					ani_once: 1
				));
			}
		}
		public static void AddAll(List<EditIactBuild> edits) {
			foreach (EditIactBuild edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Build,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Build(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, null), Entity.GetA(edit.build)),
					ani_state: 3,
					ani_once: 0
				));
			}
		}
		public static void AddAll(List<EditIactMake> edits) {
			foreach (EditIactMake edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Make,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Make(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, edit.products)),
					ani_state: 3,
					ani_once: 0
				));
			}
		}
		public static void AddAll(List<EditIactPick> edits) {
			foreach (EditIactPick edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Pick,
					time1: edit.time,
					time2: 0,
					has_dst: true,
					distance: 1,
					ef: Ef.Pick(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, edit.byproducts), edit.part, 1),
					ani_state: 3,
					ani_once: 1
				));
			}
		}
		public static void AddAll(List<EditIactRest> edits) {
			foreach (EditIactRest edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Rest,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Rest(edit.stamina),
					ani_state: 1,
					ani_once: 0
				));
			}
		}
		public static void AddAll(List<EditIactTravel> edits) {
			foreach (EditIactTravel edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					cat: ActionCategoryID.Travel,
					time1: edit.time,
					time2: 0,
					has_dst: true,
					distance: 1,
					ef: Ef.Travel(edit.stamina, edit.to),
					ani_state: 2,
					ani_once: 0
				));
			}
		}
	}
}
