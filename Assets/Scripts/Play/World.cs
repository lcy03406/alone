//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play {
	public class World {
		public interface View {
			void OnLoadGrid(Coord g, Grid grid);
			void OnUnloadGrid(Coord g);
			void OnAddEntity(Entity ent);
			void OnDelEntity(Entity ent);
		}
		public const int GRID_SIZE = 32; //must be power of 2
		public const int GRID_MASK = GRID_SIZE - 1;
		const int LOAD_RADIUS = 30;
		const int SAVE_RADIUS = 60;
		const int UNLOAD_SIZE = 20; //

		public View view;
		public WorldFile file;
		public Random rand;
		SortedList<int, Layer> layers = new SortedList<int, Layer>();
		Entity player;

		[Serializable]
		public class Param {
			public WUID maxid;
			public int time;
			public int layer;
		};
		public Param param;

		public void SetView(View view) {
			this.view = view;
		}

		public void LoadWorld(string path, string name) {
			Schema.All.Init();
			rand = new Random();
			layers.Clear();
			file = new WorldFile();
			file.LoadWorld(path, name);
			param = file.LoadParam();
			if (param == null) {
				param = new Param();
			}
			Layer layer = new Layer();
			layer.world = this;
			layer.id = param.layer;
			layers.Add(param.layer, layer);
			Entity e = file.LoadPlayer();
			if (e == null) {
				Ctx ctx = new Ctx(layer, new Coord());
				Schema.Entity.A human = Schema.Entity.GetA(Schema.Entity.ID.Human);
				e = human.CreateEntity(ctx);
				e.SetAttr(new Attrs.Ctrl());
			}
			Attrs.Pos pos = e.GetAttr<Attrs.Pos>();
			e.layer = layer;
			e.isPlayer = true;
			e.Load();
			player = e;
			layer.AddEntity(e);
		}

		public void SaveWorld() {
			file.SaveParam(param);
			file.SavePlayer(player);
			foreach (KeyValuePair<int, Layer> pair in layers) {
				pair.Value.Save();
			}
			file.SaveWorld();
		}

		public WUID NextWUID() {
			param.maxid = param.maxid.Next();
			return param.maxid;
		}

		public void Update() {
			player.Tick(param.time);
			if (param.time < player.NextTick()) {
				param.time++;
				player.Tick(param.time);
				int i = 0;
				while (i < layers.Count) {
					int id = layers.Keys[i];
					Layer layer = layers.Values[i];
					layer.Tick(param.time);
					while (i < layers.Count && layers.Keys[i] < id) {
						i++;
					}
					i++;
				}
			}
		}

		public void GoLayer(Entity ent, int to) {
			int toid = param.layer + to;
			Layer tolayer;
			if (!layers.TryGetValue(toid, out tolayer)) {
				tolayer = new Layer();
				tolayer.world = this;
				tolayer.id = toid;
			}
			Layer fromlayer = ent.layer;
			fromlayer.MoveOut(ent);
			ent.layer = tolayer;
			tolayer.MoveIn(ent);
		}
	}
}
