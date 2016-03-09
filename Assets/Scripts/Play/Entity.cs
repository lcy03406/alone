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

		public void OnLoad() {
			foreach (Attrib a in attr.Values) {
				a.ent = this;
			}
			foreach (Attrib a in attr.Values) {
				a.OnLoad();
			}
		}

		public void OnBorn() {
			foreach (Attrib a in attr.Values) {
				a.OnBorn();
			}
		}

		public void Tick (int time) {
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			if (actor != null)
				actor.Tick(time);
			Attrs.Stat stat = GetAttr<Attrs.Stat>();
			if (stat != null)
				stat.Tick(time);
			Attrs.Part part = GetAttr<Attrs.Part>();
			if (part != null)
				part.Tick(time);
			Attrs.Stage stage = GetAttr<Attrs.Stage>();
			if (stage != null)
				stage.Tick(time);
		}

		public int NextTick() {
			int next = int.MaxValue;
			Attrs.Actor actor = GetAttr<Attrs.Actor>();
			int t = (actor == null) ? int.MaxValue : actor.NextTick();
			if (t < next) next = t;
			Attrs.Stat stat = GetAttr<Attrs.Stat >();
			t = (stat == null) ? int.MaxValue : stat.NextTick();
			if (t < next) next = t;
			Attrs.Part part = GetAttr<Attrs.Part>();
			t = (part == null) ? int.MaxValue : part.NextTick();
			if (t < next) next = t;
			Attrs.Stage stage = GetAttr<Attrs.Stage>();
			t = (stage == null) ? int.MaxValue : stage.NextTick();
			if (t < next) next = t;
			return next;
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
