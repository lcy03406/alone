//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Attrs {
	[Serializable]
	public class Core : Attrib {
		public Schema.Entity.A a;

		public Core(Schema.Entity.A a) {
			this.a = a;
		}

		public Schema.EntityStage GetStage() {
			Stage stage = ent.GetAttr<Stage>();
			if (stage == null)
				return null;
			Schema.EntityStage estage;
			if (!a.s.stages.TryGetValue(stage.a.id, out estage))
				return null;
			return estage;
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
			Assert.IsNotNull(a.s.sprite, string.Format("{0} is null!", a.id));
			return a.s.sprite;
		}

		public List<Schema.Iact.A> ListMake() {
			Schema.EntityStage es = GetStage();
			if (es == null || es.make == null)
				return null;
            List<Schema.Iact.A> list = new List<Schema.Iact.A>();
			foreach (Schema.Iact.A iact in es.make) {
				//Ctx ctx = new Ctx(ent.world, ent, null);
				//if (iact.s.i.Can(ctx)) {
					list.Add(iact);
				//}
			}
			return list;
		}

		public List<Schema.Iact.A> ListIact(Entity src) {
			Schema.EntityStage es = GetStage();
			if (es == null || es.iact_dst == null)
				return null;
			List<Schema.Iact.A> list = new List<Schema.Iact.A>();
			foreach (Schema.Iact.A a in es.iact_dst) {
				Ctx ctx = new Ctx(src.layer, src, ent);
				if (a.Can(ctx)) {
					list.Add(a);
				}
			}
			return list;
		}
	}
}
