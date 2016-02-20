//utf-8。
using System;
using UnityEngine.Assertions;

namespace Play {
	[Serializable]
	public abstract class Attrib {
		[NonSerialized]
		public Entity ent;
		public virtual void SetEntity (Entity ent) {
			this.ent = ent;
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
