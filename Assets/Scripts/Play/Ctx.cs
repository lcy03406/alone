//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play {
	public sealed class Ctx {
		public class Invx {
			public List<Item> use = new List<Item>();
			public List<Item> del = new List<Item>();
		}
		public Layer layer = null;
		public Entity src = null;
		public Entity dst = null;
		public Coord dstc;
		public List<Entity> use_ent = new List<Entity>();
		public Dictionary<Inv, Invx> invs = new Dictionary<Inv, Invx>();
		public List<List<Item>> items = new List<List<Item>>();

		public Ctx(Layer layer, Entity src, Entity dst, Coord dstc) {
			this.layer = layer;
			this.src = src;
			this.dst = dst;
			this.dstc = dstc;
		}

		public Ctx(Layer layer, Coord dstc)
			: this(layer, null, null, dstc) {
		}
		public Ctx(Layer layer, Entity src)
			: this(layer, src, null, src.GetAttr<Pos>().c) {
		}
		public Ctx(Layer layer, Entity src, Entity dst)
			: this(layer, src, dst, (dst == null ? src : dst).GetAttr<Pos>().c) {
		}

		public Invx GetInv(Inv inv) {
			Invx invx;
			if (invs.TryGetValue(inv, out invx))
				return invx;
			invx = new Invx();
			invs.Add(inv, invx);
			return invx;
		}

		public void Do() {
			foreach (KeyValuePair<Inv, Invx> pair in invs) {
				pair.Key.DelItem(pair.Value.del);
			}
        }
	}
}
