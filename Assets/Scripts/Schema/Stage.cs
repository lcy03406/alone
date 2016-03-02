using System;

namespace Schema {
	public sealed class Stage : SchemaBase<Stage.ID, Stage> {
		public readonly string name;
		public readonly Iact.A[] make;
		public readonly Iact.A[] iact;
		public readonly Play.Attrs.Stat<UsageID> usage;
		public readonly Play.Effect tick_ef;

		public Stage(string name, Iact.A[] make, Iact.A[] iact, Play.Attrs.Stat<UsageID> usage, Play.Effect tick_ef) {
			this.name = name;
			this.make = make;
			this.iact = iact;
			this.usage = usage;
			this.tick_ef = tick_ef;
		}

		public enum ID {
			Boulder_Static,
		}
		static public void Init () {
			Add(ID.Boulder_Static, new Stage(name: "",
				make: null,
				iact: new Iact.A[] {
					Iact.GetA (Iact.ID.Chip_Stone),
				},
				usage: null,
				tick_ef: null //TODO
			));
		}
	}
}
