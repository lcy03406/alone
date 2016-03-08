//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public sealed class Entity {
		public WUID id;
		Dictionary<Type, Attrib> attr = new Dictionary<Type, Attrib>();

		[NonSerialized]
		public Layer layer;
		[NonSerialized]
		public bool isPlayer = false;

		public void Load() {
			foreach (Attrib at in attr.Values) {
				at.ent = this;
			}
		}

		public void Tick (int time) {
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			if (actor != null)
				actor.Tick(time);
			Attrs.Part part = GetAttr<Attrs.Part>();
			if (part != null)
				part.Tick(time);
			Attrs.Stage stage = GetAttr<Attrs.Stage>();
			if (stage != null)
				stage.Tick(time);
		}

		public int NextTick() {
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			if (actor == null)
				return 0;
			int n = 0;
			int t = actor.NextTick();
			if (t > 0) {
				if (n == 0 || t < n) {
					n = t;
				}
			}
			return n;
		}

		public void SetAttr<T>(T a) where T : Attrib {
			DelAttr<T>();
			Type cls = a.AttribClass();
			attr.Add(cls, a);
			a.ent = this;
			if (layer != null) {
				a.OnAttach();
			}
		}

		public void DelAttr<T>() {
			Type cls = Attrib.AttribClass(typeof(T));
			Attrib aa;
			if (attr.TryGetValue(cls, out aa)) {
				if (layer != null) {
					aa.OnDetach();
				}
				aa.ent = null;
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
