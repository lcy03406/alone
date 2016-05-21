//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	public sealed class ACraft : AAct {
		public struct Ingredient {
			public AItem item;
			public int count;
			public int qw;
		}
		public int time;
		public int q;
		public int qw;
		public List<Ingredient> tools;
		public List<Ingredient> ingredients;
		public List<Ingredient> condiments;
		public ItemCount product;
	}
}
