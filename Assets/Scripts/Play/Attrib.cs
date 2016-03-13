//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public abstract class Attrib {
		[NonSerialized]
		public Entity ent;
		protected int next_tick;

		public virtual void OnBorn() { }
		public virtual void OnAttach() { }
		public virtual void OnDetach() { }
		public virtual void OnLoad() { }

		public virtual void Tick(int time) { }
		public void ClrNextTick() {
			next_tick = 0;
		}
		public void SetNextTick(int time) {
			if (next_tick == 0 || next_tick > time && time > 0) {
				next_tick = time;
			}
		}
		public virtual int GetNextTick() {
			return next_tick;
		}

		public static Type AttribClass(Type cls) {
			Assert.IsTrue(cls.IsSubclassOf(typeof(Attrib)));
			while (true) {
				if (cls.BaseType == typeof(Attrib)) {
					return cls;
				}
				cls = cls.BaseType;
			}
		}
		public Type AttribClass() {
			return AttribClass(this.GetType());
		}
	}
}
