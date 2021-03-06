//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public static class EntityCreate {
		public static Entity CreateEntity(this Schema.Entity.A a, Ctx ctx, bool player = false, Attrs.Part.PartItem item = null) {
			if (a.s == null)
				return null;
			Entity ent = new Entity();
			ent.id = ctx.layer.world.NextWUID();
			ent.layer = ctx.layer;
			Attrs.Pos pos = new Attrs.Pos();
			pos.c = ctx.dstc;
			pos.dir = Direction.None;
			ent.SetAttr(pos);
			ent.SetAttr(new Attrs.Core(a));
			Schema.Entity s = a.s;
			if (s.stat != null) {
				ent.SetAttr(new Attrs.Stat(s.stat));
			}
			if (s.part != null) {
				Attrs.Part part = new Attrs.Part(s.part);
				ent.SetAttr(part);
				if (item != null) {
					part.AddPart(Schema.PartID.Item, item);
				}
			}
			if (s.start_stage != null) {
				Attrs.Stage stage = new Attrs.Stage();
				stage.a = a.s.start_stage; //TODO
				ent.SetAttr(stage);
			}
			if (s.attr != null) {
				s.attr.Create(ctx, ent);
			}
			ent.isPlayer = player;
			if (player) {
				ent.SetAttr(new Attrs.Ctrl());
			}
			ctx.layer.AddEntity(ent);
			ent.OnBorn();
			if (ent.layer != null) {
				ent.layer.AddTick(ent);
			}
			return ent;
		}
	}

	public abstract class AttrCreate {
		public abstract void Create(Ctx ctx, Entity ent);
	}
}

namespace Play.Ents {
	public class Item : AttrCreate {
		public override void Create(Ctx ctx, Entity ent) {
			Attrs.Core core = ent.GetAttr<Attrs.Core>();
			ent.SetAttr(new Attrs.CoreItem(core.a));
		}
	}
	public class Creature : AttrCreate {
		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Actor());
			ent.SetAttr(new Attrs.Inv());
			ent.SetAttr(new Attrs.AIHuman());
		}
	}
}
