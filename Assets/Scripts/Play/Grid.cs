//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class Grid {

		[Serializable]
		public class Data {
			public int time;
			public Schema.Floor.A[,] tiles = new Schema.Floor.A[World.GRID_SIZE, World.GRID_SIZE];
			public List<Entity> entities = new List<Entity> ();
		}

		public Layer layer;

		public Coord c;

		public Data d;

		public List<Entity> entities = new List<Entity> ();

		public Grid(Layer layer, Coord c) {
			this.layer = layer;
			this.c = c;
		}

		public void Load(Data d) {
			this.d = d;
			foreach (Entity ent in d.entities) {
				Assert.AreEqual(c, ent.GetAttr<Attrs.Pos>().c.Grid(), string.Format("c={0}, e={1}", c, ent.GetAttr<Attrs.Pos>().c));
				ent.layer = layer;
				ent.Load();
				layer.AddEntity(ent);
			}
			foreach (Entity ent in d.entities) {
				ent.OnLoad();
				if (ent.layer != null) {
					ent.layer.AddTick(ent);
				}
			}
			d.entities.Clear ();
		}

		public Data Save () {
			d.entities.Clear ();
			foreach (Entity e in entities) {
				Assert.AreEqual (c, e.GetAttr<Attrs.Pos>().c.Grid (), string.Format ("c={0}, e={1}", c, e.GetAttr<Attrs.Pos>().c));
				d.entities.Add (e);
			}
			return d;
		}

		public void Unload () {
			List<Entity> unload = new List<Entity>(entities);
			foreach (Entity e in unload) {
				layer.DelEntity (e);
			}
		}

		public void MoveOut (Entity e) {
			bool re = entities.Remove (e);
			Assert.IsTrue(re);
		}
		public void MoveIn (Entity e) {
			Assert.AreEqual (c, e.GetAttr<Attrs.Pos>().c.Grid (), string.Format ("c={0}, e={1}", c, e.GetAttr<Attrs.Pos>().c));
			entities.Add (e);
		}

		public List<Entity> FindEntity (Coord c, EntitySelect sel) {
			List<Entity> list = new List<Entity>();
			foreach (Entity e in entities) {
				if (e.GetAttr<Attrs.Pos>().c == c) {
					if (sel == null || sel.Select(e)) {
						list.Add(e);
					}
				}
			}
			return list;
		}
	}
}
