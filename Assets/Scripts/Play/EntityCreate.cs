//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class EntityCreate {
		Schema.Entity.A a;

		public EntityCreate(Schema.Entity.A a) {
			this.a = a;
		}

		public override string ToString() {
			return Display();
		}
		public string Display() {
			return a.s.name;
		}

		public Entity Create(Ctx ctx) {
			if (a.s == null)
				return null;
			Entity ent = Entity.Create(ctx, a);
			return ent;
		}
	}

	public abstract class AttrCreate {
		public abstract void Create(Ctx ctx, Entity ent);
	}
}

namespace Play.Ents {
	public class Static : AttrCreate {
		Attrs.Grow part;

		public Static(Attrs.Grow part) {
			this.part = part;
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Grow(part));
			ent.SetAttr(new Attrs.Stages.Static.Static());
		}
	}

	public class Tree : AttrCreate {
		Attrs.Stat<Stats.Tree> stat;
		Attrs.Grow part;

		public Tree(Attrs.Stat<Stats.Tree> stat, Attrs.Grow part) {
			this.stat = stat;
			this.part = part;
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Stat<Stats.Tree>(stat));
			ent.SetAttr(new Attrs.Grow(part));
			ent.SetAttr(new Attrs.Stages.Tree.Young());
		}
	}

	public class Creature : AttrCreate {
		Attrs.Stat<Stats.Creature> stat;
		Attrs.Grow part;

		public Creature(Attrs.Stat<Stats.Creature> stat, Attrs.Grow part)
		{
			this.stat = stat;
			this.part = part;
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Stat<Stats.Creature>(stat));
			ent.SetAttr(new Attrs.Grow(part));
			ent.SetAttr(new Attrs.Actor());
			ent.SetAttr(new Attrs.Inv());
			ent.SetAttr(new Attrs.AIHuman());
			ent.SetAttr(new Attrs.Stages.Creature.Alive());
		}
	}

	public class Workshop : AttrCreate {

		public Workshop() {
		}

		public override void Create(Ctx ctx, Entity ent) {
			ent.SetAttr(new Attrs.Stages.Workshop.Off());
		}
	}
}
