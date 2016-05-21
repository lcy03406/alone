//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	//TODO
	public sealed class AFloor : Meta {
		public Text name;
		public ATile tile;
	}
	public sealed class Biome : Meta {
		public Text name;
		public List<Fun.Choice<AFloor>> floors;
		public List<Fun.Choice<AEntity>> ents;
	}
}

