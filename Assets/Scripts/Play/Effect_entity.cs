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

		static EntitySelect SelectBlockade = new EntitySelect() { blockade = 1 };
		public override bool Can(Ctx ctx) {
			Pos pos = ctx.src.GetAttr<Pos>();
			if (pos == null)
				return false;
			Coord c = pos.c.Step(pos.dir);
			if (ctx.layer.SearchEntity(c, SelectBlockade).Count > 0)
				return false;
			ctx.dstc = c; //TODO
			if (!c_cre.Can(ctx))
				return false;
			return true;
		}

		public override void Do(Ctx ctx, List<string> logs) {
			Schema.Entity.A cre = c_cre.Get(ctx);
			Entity cent = cre.CreateEntity(ctx);
			if (logs != null) {
				logs.Add(cent.GetName() + " arise.");
			}
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			if (logs != null) {
				logs.Add(ent.GetName() + " no longer exist.");
			}
			ctx.layer.DelEntity(ent);
		}
	}

	public class Dir : Effect {
		public readonly Calc<Entity> c_ent;

		public Dir(Calc<Entity> ent) {
			c_ent = ent;
		}

		public override string Display() {
			string disp = c_ent.Display() + ": change direction.\n";
			return disp;
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Pos pos = ent.GetAttr<Pos>();
			Direction dir = ctx.dstc.Sub(pos.c).ToDirection();
			if (dir == Direction.None || dir == Direction.Center)
				return false;
			if (dir == pos.dir)
				return false;
			return true;
		}

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Pos pos = ent.GetAttr<Pos>();
			Direction dir = ctx.dstc.Sub(pos.c).ToDirection();
			pos.dir = dir;
			if (logs != null) {
				logs.Add(ent.GetName() + " turn " + dir + ".");
			}
		}
	}

	public class Move : Effect {
		public readonly Calc<Entity> c_ent;

		public Move(Calc<Entity> ent) {
			c_ent = ent;
		}

		public override string Display() {
			string disp = c_ent.Display() + ": ";
			disp += "move forward";
			return disp + ".\n";
		}

		public override bool Can(Ctx ctx) {
			Entity ent = c_ent.Get(ctx);
			if (ent == null)
				return false;
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			if (pos.dir == Direction.None || pos.dir == Direction.Center)
				return false;
			Coord tc = pos.c.Step(pos.dir);
			Layer layer = ent.layer;
			return layer.CanMoveTo(tc);
		}

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Attrs.Pos pos = ent.GetAttr<Attrs.Pos>();
			Coord tc = pos.c.Step(pos.dir);
			Layer layer = ent.layer;
			layer.MoveEntity(ent, tc);
			if (logs != null) {
				logs.Add(ent.GetName() + " go to " + tc + ".");
			}
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			ctx.layer.world.GoLayer(ent, to);
			if (logs != null) {
				logs.Add(ent.GetName() + " go to layer " + ent.layer.z);
			}
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

		public override void Do(Ctx ctx, List<string> logs) {
			Entity ent = c_ent.Get(ctx);
			Stage stage = ent.GetAttr<Stage>();
			if (logs != null) {
				logs.Add(ent.GetName() + " become " + to.s.name);
			}
			stage.Transit(to, logs);
		}
	}
}
