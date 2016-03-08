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
			foreach (Entity e in d.entities) {
				Assert.AreEqual (c, e.GetAttr<Attrs.Pos>().c.Grid (), string.Format ("c={0}, e={1}", c, e.GetAttr<Attrs.Pos>().c));
				e.layer = layer;
				layer.AddEntity (e);
				e.OnLoad();
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

		public void Update (int time) {
		}

		public void MoveOut (Entity e) {
			bool re = entities.Remove (e);
			Assert.IsTrue(re);
		}
		public void MoveIn (Entity e) {
			Assert.AreEqual (c, e.GetAttr<Attrs.Pos>().c.Grid (), string.Format ("c={0}, e={1}", c, e.GetAttr<Attrs.Pos>().c));
			entities.Add (e);
		}

		public Entity FindEntity (Coord c) {
			foreach (Entity e in entities) {
				if (e.GetAttr<Attrs.Pos>().c == c) {
					return e;
				}
			}
			return null;
		}
	}
}
