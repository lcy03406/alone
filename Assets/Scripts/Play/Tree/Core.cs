//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Tree {
	[Serializable]
	public class Core : Play.Core {
		public Schema.Tree.A race;

		public static Entity CreateEntity (World world, Schema.Tree.A race) {
			Core core = new Core {
				race = race,
			};
			Entity e = new Entity {
				id = world.NextWUID (),
			};
			e.SetAttr (core);
			e.SetAttr (new Show ());
			e.SetWorld (world);
			return e;
		}

		static Schema.Iact.A[] iacts = {
			Schema.Iact.GetA (Schema.Iact.ID.Tree_PickBranch),
            Schema.Iact.GetA (Schema.Iact.ID.Tree_PickFruit),
		};

		public override List<Schema.Iact.A> ListIact (Entity src) {
			List<Schema.Iact.A> list = new List<Schema.Iact.A> ();
			foreach (Schema.Iact.A iact in iacts) {
				if (iact.s.i.Can(src, ent)) {
					list.Add (iact);
				}
			}
			return list;
		}
	}
}
