//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public sealed class Entity {
		Dictionary<Type, Attrib> attr = new Dictionary<Type, Attrib> ();
		public WUID id;
		public Coord c;
		public Direction dir;

		[NonSerialized]
		public World world;
		//private WorldGrid grid;
		[NonSerialized]
		public bool isPlayer = false;

		public static Entity Create(Ctx ctx, Schema.Entity.A a) {
			Entity ent = new Entity();
			ent.id = ctx.world.NextWUID();
			ent.SetAttr(new Attrs.Core(a));
			AttrCreate attr = a.s.attr;
			if (attr != null)
				attr.Create(ctx, ent);
			ent.dir = Direction.None;
			ent.SetWorld(ctx.world);
			return ent;
		}


		public void SetWorld (World world) {
			this.world = world;
			foreach (Attrib a in attr.Values) {
				a.SetEntity (this);
			}
		}

		public void Tick (int time) {
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			if (actor != null)
				actor.Tick(time);
			Attrs.Grow grow = GetAttr<Attrs.Grow>();
			if (grow != null)
				grow.Tick(time);
			Attrs.Stage stage = GetAttr<Attrs.Stage>();
			if (stage != null)
				stage.Tick(time);
		}

		public int NextTick () {
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			if (actor == null)
				return 0;
			int n = 0;
			int t = actor.NextTick ();
			if (t > 0) {
				if (n == 0 || t < n) {
					n = t;
				}
			}
			return n;
		}

		public void SetAttr<T>(T a) where T : Attrib {
			Type cls = a.AttribClass();
			Attrib aa;
			if (attr.TryGetValue(cls, out aa)) {
				aa.SetEntity(null);
				attr.Remove(cls);
			}
			attr.Add(cls, a);
			a.SetEntity(this);
		}

		public void DelAttr<T>() {
			Type cls = Attrib.AttribClass(typeof(T));
			Attrib aa;
			if (attr.TryGetValue(cls, out aa)) {
				aa.SetEntity(null);
				attr.Remove(cls);
			}
		}

		public T GetAttr<T> () where T : Attrib {
			Type cls = Attrib.AttribClass(typeof(T));
			Attrib a;
			if (!attr.TryGetValue(cls, out a))
				return null;
			return a as T;
		}
	}
}
