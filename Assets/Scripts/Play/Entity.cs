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

		public void SetWorld (World world) {
			this.world = world;
			foreach (Attrib a in attr.Values) {
				a.SetEntity (this);
			}
		}

		public void Tick (int time) {
			foreach (Attrib a in attr.Values) {
				a.Tick (time);
			}
		}

		public int NextTick () {
			int n = 0;
			foreach (Attrib a in attr.Values) {
				int t = a.NextTick ();
				if (t > 0) {
					if (n == 0 || t < n) {
						n = t;
					}
				}
			}
			return n;
		}

		private static Type AttribClass(Type cls) {
			Assert.IsTrue (cls.IsSubclassOf (typeof (Attrib)));
			while (true) {
				if (cls.BaseType == typeof (Attrib)) {
					return cls;
				}
				cls = cls.BaseType;
			}
		}

		public void SetAttr(Attrib a) {
			Type cls = AttribClass (a.GetType ());
			Attrib aa;
			if (attr.TryGetValue(cls, out aa)) {
				aa.SetEntity(null);
				attr.Remove(cls);
			}
			attr.Add(cls, a);
			a.SetEntity(this);
		}

		public T GetAttr<T> () where T : Attrib {
			Type cls = AttribClass (typeof(T));
			Attrib a;
			if (!attr.TryGetValue(cls, out a))
				return null;
			return a as T;
		}
	}
}
	