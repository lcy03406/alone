//utf-8。
using System;
using System.Collections.Generic;

namespace Play {
	public sealed class Ctx {
		public Entity src = null;
		public Entity dst = null;
		public List<List<Item>> items = new List<List<Item>>();
	}

	public interface Calc<T> {
		bool Can(Ctx ctx);
		T Get(Ctx ctx);
	}

	public class Const<T> : Calc<T> {
		public T t;
		public Const(T t) {
			this.t = t;
		}
		public bool Can(Ctx ctx) {
			return true;
		}
		public T Get(Ctx ctx) {
			return t;
		}
	}
	public class Src : Calc<Entity> {
		public bool Can(Ctx ctx) {
			return true;
		}
		public Entity Get(Ctx ctx) {
			return ctx.src;
		}
	}

	public class Dst : Calc<Entity> {
		public bool Can(Ctx ctx) {
			return true;
		}
		public Entity Get(Ctx ctx) {
			return ctx.dst;
		}
	}
}
