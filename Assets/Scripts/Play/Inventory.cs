//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	[Serializable]
	public class Inventory : Attrib{
		public List<Item> items = new List<Item> ();
	}
}