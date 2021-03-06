using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Schema {
	public class SchemaBase<ID, Data> where ID : struct where Data : SchemaBase<ID, Data> {
		static private SortedList<ID, Data> data = new SortedList<ID, Data> ();
		protected SchemaBase() {
		}
		static private Data Get (ID id) {
			Data datum = null;
			data.TryGetValue (id, out datum);
			return datum;
		}

		static protected void Add (ID id, Data datum) {
			Assert.IsFalse (data.ContainsKey (id), id.ToString());
			data.Add (id, datum);
		}

		public static A GetA(ID id) {
			A a = new A(id);
			return a;
		}

		[Serializable]
		public class A {
			public ID id;
			[NonSerialized]
			private Data data;
			
			public Data s {
				get {
					if (data == null) {
						data = Get (id);
					}
					return data;
				}
			}

			public A (ID id) {
				this.id = id;
				this.data = null;
			}

			public override string ToString() {
				return s.ToString();
			}

			public override bool Equals(object obj) {
				if (obj == null)
					return false;
				if (!(obj is A))
					return false;
				A b = (A)obj;
				return this.id.Equals(b.id);
			}

			public override int GetHashCode() {
				return this.id.GetHashCode();
			}

			public bool Equals(A b) {
				if ((object)b == null)
					return false;
				return this.id.Equals(b.id);
			}

			public static bool operator == (A a, A b) {
				if ((object)a == null)
					return (object)b == null;
				return a.Equals(b);
			}

			public static bool operator != (A a, A b) {
				return !(a == b);
			}

		}
	}
}

