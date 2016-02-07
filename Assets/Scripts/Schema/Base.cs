using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Schema {
	public class SchemaBase<ID, Data> where ID : struct where Data : SchemaBase<ID, Data> {
		static private SortedList<ID, Data> data = new SortedList<ID, Data> ();
		static private Data Get (ID id) {
			Data datum = null;
			bool ret = data.TryGetValue (id, out datum);
			Assert.IsTrue (ret);
			return datum;
		}

		static protected void Add (ID id, Data datum) {
			Assert.IsFalse (data.ContainsKey (id));
			data.Add (id, datum);
		}

		[Serializable]
		public struct A {
			public readonly ID id;
			[NonSerialized]
			private Data data;
			
			public Data s {
				get {
					if (data == null) {
						data = SchemaBase<ID, Data>.Get (id);
						Assert.IsNotNull (data);
					}
					return data;
				}
			}
			public A (ID id) {
				this.id = id;
				this.data = null;
			}
		}

		public static A GetA (ID id) {
			return new A (id);
		}
	}
}

