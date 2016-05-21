//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public sealed class AItem : Meta, HasName {
		public Text name { get; set; }
		public Text namepl { get; set; }
		public AItem cat;
		public ATile icon;
	}

	public struct ItemCount {
		public AItem item;
		public int count;
	}
}
