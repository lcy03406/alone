//utf-8。
using System;

using T = System.UInt64;

namespace Play {
	[Serializable]
	public struct WUID : IComparable<WUID> {
		public static readonly WUID None = new WUID();

		public readonly T value;

		public WUID(ulong value) {
			this.value = value;
		}

		public WUID Next() {
			return new WUID(value + 1);
		}

		public override int GetHashCode() {
			return this.value.GetHashCode();
		}

		public override string ToString() {
			return this.value.ToString();
		}

		public override bool Equals(object obj) {
			if (obj == null)
				return false;
			if (!(obj is WUID))
				return false;
			WUID b = (WUID)obj;
			return this.value == b.value;
		}
		public bool Equals(WUID b) {
			if ((object)b == null)
				return false;
			return this.value == b.value;
		}

		public int CompareTo(WUID b) {
			if (value < b.value)
				return -1;
			else if (value > b.value)
				return 1;
			else
				return 0;
		}

		public static bool operator == (WUID a, WUID b) {
			return a.value == b.value;
		}

		public static bool operator != (WUID a, WUID b) {
			return a.value != b.value;
		}

		public static bool operator < (WUID a, WUID b) {
			return a.value < b.value;
		}

		public static bool operator > (WUID a, WUID b) {
			return a.value > b.value;
		}
	}
}
