//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public abstract class EntityCreate {
		Schema.Entity.A a;

		public Entity Create(Ctx ctx) {
			Entity ent = new Entity();
			ent.id = ctx.world.NextWUID();
			CreateAttrs(ctx, ent);
			ent.dir = Direction.None;
			ent.SetWorld(ctx.world);
			return ent;
		}

		public abstract void CreateAttrs(Ctx ctx, Entity ent);
	}
}

namespace Play.Ents {
	public class Creature : EntityCreate {
		Schema.Entity.A a;
		Attrs.Stat<Stats.Creature> stat;

		public Creature(Schema.Entity.A a,
			Attrs.Stat<Stats.Creature> stat)
		{
			this.a = a;
			this.stat = stat;
		}

		public override void CreateAttrs(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Core(a));
			ent.SetAttr(new Attrs.Stat<Stats.Creature>(stat));
			ent.SetAttr(new Attrs.Actor());
			ent.SetAttr(new Attrs.Inv());
			ent.SetAttr(new Attrs.AIHuman());
		}
	}
	public class Tree : EntityCreate {
		Schema.Entity.A a;
		Attrs.Stat<Stats.Tree> stat;
		Attrs.Part<Parts.Tree> part;

		public Tree(Schema.Entity.A a,
			Attrs.Stat<Stats.Tree> stat,
			Attrs.Part<Parts.Tree> part)
		{
			this.a = a;
			this.stat = stat;
			this.part = part;
		}

		public override void CreateAttrs(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Core(a));
			ent.SetAttr(new Attrs.Stat<Stats.Tree>(stat));
			ent.SetAttr(new Attrs.Part<Parts.Tree>(part));
		}
	}
}
