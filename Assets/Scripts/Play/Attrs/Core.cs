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

		public virtual string GetName() {
			return a.s.name;
		}

		public virtual Schema.Sprite.A GetSprite() {
			Assert.IsNotNull(a.s.sprite, string.Format("{0} is null!", a.id));
			return a.s.sprite;
		}

		public bool GetBlockade() {
			Stage stage = ent.GetAttr<Stage>();
			if (stage == null)
				return true;
			return stage.a.s.blockade;
		}

		public Schema.RenderOrderID GetRenderLayer() {
			Stage stage = ent.GetAttr<Stage>();
			if (stage == null)
				return 0;
			return stage.a.s.render_order;
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
				if (a.s.cat == Schema.ActionCategoryID.Make 
					|| a.s.cat == Schema.ActionCategoryID.Build 
					|| a.Can(ctx)) {
					list.Add(a);
				}
			}
			return list;
		}

		public Schema.Iact.A GetIactAuto(Entity src) {
			Schema.EntityStage es = GetStage();
			if (es == null || es.iact_auto == null)
				return null;
			Schema.Iact.A a = es.iact_auto;
			Ctx ctx = new Ctx(src.layer, src, ent);
			if (a.Can(ctx)) {
				return a;
			}
			return null;
		}
	}

	[Serializable]
	public class CoreItem : Core {
		public CoreItem(Schema.Entity.A a) : base(a) {
		}
		private Schema.Item.A GetItem() {
			Part grow = ent.GetAttr<Part>();
			if (grow != null) {
				Part.PartItem part = grow.Get(Schema.PartID.Item);
				if (part != null) {
					return part.a;
				}
			}
			return null;
		}
		public override string GetName() {
			Schema.Item.A a = GetItem();
			if (a == null)
				return base.GetName();
			return GetItem().s.name;
		}
		public override Schema.Sprite.A GetSprite() {
			Schema.Item.A a = GetItem();
			if (a == null)
				return base.GetSprite();
			return a.s.sprite;
		}
	}
}
