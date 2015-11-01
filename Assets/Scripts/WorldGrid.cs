using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class WorldGrid {

	[Serializable]
	public class Data {
		public int time;
		public int[,] tiles = new int[World.GRID_SIZE, World.GRID_SIZE];
		public List<WorldEntity.Data> entities = new List<WorldEntity.Data>();
	}
	
	public World world;

	public Coord c;
	
	public Data d;
	
	public List<WorldEntity> entities = new List<WorldEntity> ();

	public WorldGrid (World w, Coord c, Data d) {
		this.world = w;
		this.c = c;
		this.d = d;
		foreach (WorldEntity.Data ed in d.entities) {
			WorldEntity e = new WorldEntity (world, this, ed);
			Assert.AreEqual (c, e.d.c.Grid (), string.Format ("c={0}, e={1}", c, e.d.c));
			entities.Add (e);
			world.AddEntity (e);
		}
		d.entities.Clear ();
	}

	public Data Save () {
		d.entities.Clear ();
		foreach (WorldEntity e in entities) {
			Assert.AreEqual (c, e.d.c.Grid (), string.Format ("c={0}, e={1}", c, e.d.c));
			WorldEntity.Data ed = e.Save ();
			d.entities.Add (ed);
		}
		return d;
	}

	public void Unload () {
		foreach (WorldEntity e in entities) {
			world.DelEntity (e);
		}
	}

	public void Update (int time) {
	}

	public bool MoveOut (WorldEntity e) {
		Assert.AreNotEqual (c, e.d.c.Grid (), string.Format ("c={0}, e={1}", c, e.d.c));
		return entities.Remove (e);
	}
	public void MoveIn (WorldEntity e) {
		Assert.AreEqual (c, e.d.c.Grid (), string.Format ("c={0}, e={1}", c, e.d.c));
		entities.Add (e);
	}

	public WorldEntity FindEntity (Coord c) {
		foreach (WorldEntity e in entities) {
			if (e.d.c == c) {
				return e;
			}
		}
		return null;
	}
}
