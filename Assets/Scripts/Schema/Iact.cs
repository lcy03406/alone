//utf-8。

using System.Collections.Generic;

namespace Schema {
	public sealed partial class Iact : SchemaBase<ActionID, Iact> {
		public readonly string name;
		public readonly int time1;
		public readonly int time2;
		public readonly bool has_dst;
		public readonly int distance;
		public readonly Play.Effect ef;

		public override string ToString() {
			return Play.Iact.Display(this);
		}

		private Iact(string name, int time1, int time2, bool has_dst, int distance, Play.Effect ef) {
			this.name = name;
			this.time1 = time1;
			this.time2 = time2;
			this.has_dst = has_dst;
			this.distance = distance;
			this.ef =ef;
		}

		public static void AddAll(List<EditIactMove> edits) {
			foreach (EditIactMove edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time1,
					time2: edit.time2,
					has_dst: true,
					distance: 1,
					ef: Ef.Move(edit.stamina)
				));
			}
		}
		public static void AddAll(List<EditIactAttack> edits) {
			foreach (EditIactAttack edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time1,
					time2: edit.time2,
					has_dst: true,
					distance: 1,
					ef: Ef.Attack(edit.stamina, edit.mulDamage, edit.addDamage) //TODO
				));
			}
		}
		public static void AddAll(List<EditIactBuild> edits) {
			foreach (EditIactBuild edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Build(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, null), Entity.GetA(edit.build))
				));
			}
		}
		public static void AddAll(List<EditIactMake> edits) {
			foreach (EditIactMake edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Make(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, edit.products))
				));
			}
		}
		public static void AddAll(List<EditIactPick> edits) {
			foreach (EditIactPick edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time,
					time2: 0,
					has_dst: true,
					distance: 1,
					ef: Ef.Pick(Ef.MakeCommon(edit.stamina, edit.tools, edit.reagents, edit.byproducts), edit.part, 1)
				));
			}
		}
		public static void AddAll(List<EditIactRest> edits) {
			foreach (EditIactRest edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time,
					time2: 0,
					has_dst: false,
					distance: 0,
					ef: Ef.Rest(edit.stamina)
				));
			}
		}
		public static void AddAll(List<EditIactTravel> edits) {
			foreach (EditIactTravel edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time,
					time2: 0,
					has_dst: true,
					distance: 1,
					ef: Ef.Travel(edit.stamina, edit.to)
				));
			}
		}
	}
}
