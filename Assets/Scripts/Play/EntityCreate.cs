//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public sealed class EntityCreate {
		Schema.Entity.A a;

		public EntityCreate(Schema.Entity.A a) {
			this.a = a;
		}

		public Entity Create(Ctx ctx) {
			Entity ent = Entity.Create(ctx, a);
			return ent;
		}
	}

	public abstract class AttrCreate {
		public abstract void Create(Ctx ctx, Entity ent);
	}
}

namespace Play.Ents {
	public class Creature : AttrCreate {
		Attrs.Stat<Stats.Creature> stat;

		public Creature(Attrs.Stat<Stats.Creature> stat)
		{
			this.stat = stat;
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Stat<Stats.Creature>(stat));
			ent.SetAttr(new Attrs.Actor());
			ent.SetAttr(new Attrs.Inv());
			ent.SetAttr(new Attrs.AIHuman());
		}
	}

	public class Tree : AttrCreate {
		Attrs.Stat<Stats.Tree> stat;
		Attrs.Part<Parts.Tree> part;

		public Tree(Attrs.Stat<Stats.Tree> stat,
			Attrs.Part<Parts.Tree> part)
		{
			this.stat = stat;
			this.part = part;
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Stat<Stats.Tree>(stat));
			ent.SetAttr(new Attrs.Part<Parts.Tree>(part));
		}
	}

	public class Workshop : AttrCreate {

		public Workshop() {
		}

		public override void Create(Ctx ctx, Entity ent) {
		}
	}
}
