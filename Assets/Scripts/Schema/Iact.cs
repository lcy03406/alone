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

		public static void AddAll(List<EditIactAttack> edits) {
			foreach (EditIactAttack edit in edits) {
				Add(edit.id, new Iact(
					name: edit.name,
					time1: edit.time1,
					time2: edit.time2,
					has_dst: true,
					distance: 1,
					ef: Ef.Attack(edit.stamina, new Play.Calcs.GetStat(new Play.Calcs.Src(), StatID.Attack)) //TODO
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
					has_dst: false,
					distance: 0,
					ef: Ef.Travel(edit.stamina, edit.to)
				));
			}
		}
		/*
				public static Iact Attack(string name, int time1, int time2, int sta,
					Play.Calc<int> damage) {
					return new Iact(
						name: name,
						time1: time1,
						time2: time2,
						has_dst: true,
						distance: 1,
						ef: Ef.Attack(sta, damage)
					);
				}

				public static Iact Travel(string name, int time1, int time2, int sta, int to) {
					return new Iact(
						name: name,
						time1: time1,
						time2: time2,
						has_dst: true,
						distance: 1,
						ef: Ef.Travel(sta, to)
					);
				}

				public static Iact Pick(string name, int time1, int time2, int sta,
					Schema.PartID part, int count) {
					return new Iact(
						name: name,
						time1: time1,
						time2: time2,
						has_dst: true,
						distance: 1,
						ef: Ef.Pick(sta, part, count)
					);
				}

				public static Iact Butcher(string name, int time1, int time2, int sta,
					Schema.PartID part, int count) {
					return new Iact(
						name: name,
						time1: time1,
						time2: time2,
						has_dst: true,
						distance: 1,
						ef: Ef.Butcher(sta, part, count)
					);
				}
		
		public static Iact Make(string name, int time1, int time2, int sta,
			Play.ItemSelect[] tools,
			Play.ItemSelect[] reagents,
			Play.ItemCreate[] products,
			Schema.Entity.A build) {
			return new Iact(
				name: name,
				time1: time1,
				time2: time2,
				has_dst: false,
				distance: 0,
				ef: Ef.Make(sta, tools, reagents, products, build)
			);
		}
		*/
	}
}
