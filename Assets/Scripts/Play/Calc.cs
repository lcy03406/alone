//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play {
	public abstract class Calc<T> {
		public abstract string Display();
		public abstract bool Can(Ctx ctx);
		public abstract T Get(Ctx ctx);

		public override string ToString() {
			return Display();
		}
	}
}

namespace Play.Calcs {
	public class None<T> : Calc<T> {
		public None() {
		}
		public override string Display() {
			return "null";
		}
		public override bool Can(Ctx ctx) {
			return false;
		}
		public override T Get(Ctx ctx) {
			Assert.IsTrue(false);
			return default(T);
		}
	}

	public class Const<T> : Calc<T> {
		public T t;
		public Const(T t) {
			this.t = t;
		}
		public override string Display() {
			return t.ToString();
		}
		public override bool Can(Ctx ctx) {
			return true;
		}
		public override T Get(Ctx ctx) {
			return t;
		}
	}

	public class RandConst<T> : Calc<T> {
		public List<Fun.Choice<T>> choices;
        public RandConst(List<Fun.Choice<T>> choices) {
			this.choices = choices;
		}
		public override string Display() {
			string disp = "any of { ";
			foreach (Fun.Choice<T> choice in choices) {
				disp += choice.value;
				disp += ", ";
			}
			disp += "}";
			return disp;
		}
		public override bool Can(Ctx ctx) {
			return true;
		}
		public override T Get(Ctx ctx) {
			return Fun.Choice<T>.Choose(choices);
		}
	}

	public class Src : Calc<Entity> {
		public override string Display() {
			return "yourself";
		}
        public override bool Can(Ctx ctx) {
			return true;
		}
		public override Entity Get(Ctx ctx) {
			return ctx.src;
		}
	}

	public class Dst : Calc<Entity> {
		public override string Display() {
			return "the target";
		}
		public override bool Can(Ctx ctx) {
			return true;
		}
		public override Entity Get(Ctx ctx) {
			return ctx.dst;
		}
	}

	public class GetStat : Calc<int> {
		public readonly Calc<Entity> c_ent;
		public readonly Edit.AStat id;
		public GetStat(Calc<Entity> ent, Edit.AStat id) {
			this.c_ent = ent;
			this.id = id;
		}

		public override string Display() {
			return id.ToString() + " of " + c_ent.Display();
		}
		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stat stat = ent.GetAttr<Stat>();
			if (stat == null)
				return false;
			return true;
		}

		public override int Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stat stat = ent.GetAttr<Stat>();
			return stat.Get(id);
		}
	}

	public class Part : Calc<ItemCreate> {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.PartID id;
		public Part(Calc<Entity> ent, Schema.PartID id) {
			this.c_ent = ent;
			this.id = id;
		}

		public override string Display() {
			return id.ToString() + " of " + c_ent.Display();
		}
		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Attrs.Part grow = ent.GetAttr<Attrs.Part>();
			if (grow == null)
				return false;
			Attrs.Part.PartItem part = grow.Get(this.id);
			if (part == null)
				return false;
			return true;
		}

		public override ItemCreate Get(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Attrs.Part grow = ent.GetAttr<Attrs.Part>();
			ItemCreate cre = grow.Get(id).GetItem();
			return cre;
		}
	}

	public class ItemCount : Calc<ItemCreate> {
		Calc<ItemCreate> c_cre;
		Calc<int> c_count;

		public ItemCount(Calc<ItemCreate> cre, Calc<int> count) {
			c_cre = cre;
			c_count = count;
		}

		public override string Display() {
			return c_cre.Display() + " x " + c_count.Display();
		}
		public override bool Can(Ctx ctx) {
			return c_cre.Can(ctx) && c_count.Can(ctx);
		}
		public override ItemCreate Get(Ctx ctx) {
			ItemCreate cre = c_cre.Get(ctx);
			cre.count = c_count.Get(ctx);
			return cre;
		}
	}
}
