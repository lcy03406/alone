//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public abstract class Attrib {
		[NonSerialized]
		public Entity ent;

		public virtual void OnBorn() { }
		public virtual void OnAttach() { }
		public virtual void OnDetach() { }

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
