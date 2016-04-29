//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Pos : Attrib{
		public Coord c;
		public Direction dir;

	}
}
