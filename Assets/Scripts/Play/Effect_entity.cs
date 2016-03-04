//utf-8。
using System;
using System.Collections.Generic;
using Play.Attrs;

namespace Play.Eff {
	public class AddEntity : Effect {
		public readonly Calc<Schema.Entity.A> c_cre;

		public AddEntity(Calc<Schema.Entity.A> cre) {
			c_cre = cre;
		}

		public override string Display() {
			return "build " + c_cre.Display() + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Pos pos = ctx.src.GetAttr<Pos>();
			if (pos == null)
				return false;
			Coord c = pos.c.Step(pos.dir);
			if (ctx.layer.SearchEntity(c) != null)
				return false;
			if (!c_cre.Can(ctx))
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Pos pos = ctx.src.GetAttr<Pos>();
			//TODO
			Coord c = pos.c.Step(pos.dir);
			Schema.Entity.A cre = c_cre.Get(ctx);
			Entity e = cre.CreateEntity(ctx);
			pos.c = c;
			ctx.layer.AddEntity(e);
		}
	}

	public class DelEntity : Effect {
		public readonly Calc<Entity> c_ent;

		public DelEntity(Calc<Entity> ent) {
			c_ent = ent;
		}

		public override string Display() {
			return "destroy " + c_ent.Display() + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			ctx.layer.DelEntity(ent);
		}
	}

	public class GoLayer : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly int to;

		public GoLayer(Calc<Entity> ent, int to) {
			c_ent = ent;
			this.to = to;
		}

		public override string Display() {
			string disp = c_ent.Display() + ": ";
			if (to == 1)
				disp += "go up";
			else if (to == -1)
				disp += "go down";
			else if (to > 1)
				disp += "go " + to + " layers up";
			else if (to < -1)
				disp += "go " + -to + " layers down";
			return disp + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			ctx.layer.world.GoLayer(ent, to);
		}
	}

	public class ToStage : Effect {
		public readonly Calc<Entity> c_ent;
		public readonly Schema.Stage.A to;

		public ToStage(Calc<Entity> ent, Schema.Stage.A to) {
			c_ent = ent;
			this.to = to;
		}

		public override string Display() {
			string disp = c_ent.Display() + ": become ";
			disp += to.s.name;
			return disp + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Stage stage = ent.GetAttr<Stage>();
			if (stage == null)
				return false;
			return true;
		}

		public override void Do(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			Stage stage = ent.GetAttr<Stage>();
			stage.Transit(to);
		}
	}
}
