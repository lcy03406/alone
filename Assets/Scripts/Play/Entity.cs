//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play {
	[Serializable]
	public sealed class Entity {
		[NonSerialized]
		public Layer layer;
		[NonSerialized]
		public bool isPlayer = false;

		public WUID id;
		Dictionary<Type, Attrib> attr = new Dictionary<Type, Attrib>();

		public void Load() {
			foreach (Attrib a in attr.Values) {
				a.ent = this;
			}
		}
		public void OnLoad() {
			foreach (Attrib a in attr.Values) {
				a.OnLoad();
			}
		}

		public void OnBorn() {
			foreach (Attrib a in attr.Values) {
				a.OnBorn();
			}
		}

		static Type[] TickAttr = {
			typeof(Attrs.Actor),
			typeof(Attrs.Stat),
			typeof(Attrs.Part),
			typeof(Attrs.Stage),
		};

		public void Tick (int time, List<string> logs) {
			foreach (Type t in TickAttr) {
				Attrib a = GetAttr(t);
				if (a != null) {
					int next_tick = a.GetNextTick();
					if (next_tick > time)
						continue;
					a.ClrNextTick();
					a.Tick(time, logs);
				}
			}
		}

		public int GetNextTick() {
			int next = 0;
			foreach (Type t in TickAttr) {
				Attrib a = GetAttr(t);
				if (a != null) {
					int next_tick = a.GetNextTick();
					if (next == 0 || next_tick != 0 && next_tick < next) {
						next = next_tick;
					}
				}
			}
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
			Type t = Attrib.AttribClass(typeof(T));
			return GetAttr(t) as T;
		}

		private Attrib GetAttr(Type t) {
			Attrib a;
			if (!attr.TryGetValue(t, out a))
				return null;
			return a;
		}

		public void Log(string info) {
			Attrs.Pos pos = GetAttr<Attrs.Pos>();
			Coord c = pos == null ? Coord.O : pos.c;
			layer.Log(c, info);
		}

		public string GetName() {
			Attrs.Core core = GetAttr<Attrs.Core>();
			if (core != null)
				return core.GetName();
			return "someone";
		}
	}
}
