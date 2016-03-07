//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

using ID = Schema.PartID;

namespace Play.Attrs {
	[Serializable]
	public class Buff : Attrib {
		[Serializable]
		public class Buf {
			public Schema.BufID id;
			public int value;
			public int end_time;
		}

		public Dictionary<Schema.StatID, Buf> bufs;

		public bool AddBuf(Schema.BufID id, Dictionary<Schema.StatID, int> stats) {
			foreach (KeyValuePair<Schema.StatID, int> pair in stats) {
				Schema.StatID st = pair.Key;
				int value = pair.Value;

			}
			return false;
        }
	}
}
