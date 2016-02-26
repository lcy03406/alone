//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Attrs {
	[Serializable]
	public class Core : Attrib {
		public Schema.Entity.A a;

		public Core(Schema.Entity.A a) {
			this.a = a;
		}

		Schema.Stage GetStage() {
			Stage stage = ent.GetAttr<Stage>();
			if (stage == null)
				return null;
			Schema.Stage sstage;
			if (!a.s.stages.TryGetValue(stage.GetType(), out sstage))
				return null;
			return sstage;
		}

		public Schema.Sprite.A GetSprite() {
			//TODO
#if false
			if (stat.hp <= 0) {
				return Schema.Sprite.GetA(Schema.SpriteID.d_shovel);
			}
			Actor actor = ent.GetAttr<Actor>();
			if (actor != null) {
				Act act = actor.act;
				if (act != null) {
					//TODO
					if (act is Acts.ActMove) {
						return Schema.Sprite.GetA(Schema.SpriteID.d_boots);
					} else if (act is Acts.ActIact) {
						//TODO
						Acts.ActIact iact = (Acts.ActIact)act;
						if (iact.a.id == Schema.Iact.ID.Attack_Punch) {
							return Schema.Sprite.GetA(Schema.SpriteID.d_gauntlets);
						}
					} else {
						//return Schema.Sprite.GetA(Schema.SpriteID.d_helm);
					}
				}
			}
#endif
			return a.s.sprite;
		}

		public List<Schema.Iact.A> ListMake() {
			Schema.Stage ss = GetStage();
			if (ss == null || ss.make == null)
				return null;
            List<Schema.Iact.A> list = new List<Schema.Iact.A>();
			foreach (Schema.Iact.A iact in ss.make) {
				Ctx ctx = new Ctx(ent.world, ent, null);
				if (iact.s.i.Can(ctx)) {
					list.Add(iact);
				}
			}
			return list;
		}

		public List<Schema.Iact.A> ListIact(Entity src) {
			Schema.Stage ss = GetStage();
			if (ss == null || ss.iact == null)
				return null;
			List<Schema.Iact.A> list = new List<Schema.Iact.A>();
			foreach (Schema.Iact.A iact in ss.iact) {
				Ctx ctx = new Ctx(src.world, src, ent);
				if (iact.s.i.Can(ctx)) {
					list.Add(iact);
				}
			}
			return list;
		}
	}
}
