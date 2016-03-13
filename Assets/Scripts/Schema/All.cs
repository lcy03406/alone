using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Schema {
	public class All {
		static public void Init() {
			Sprite.Init();
			LoadAll();
			/*
			Floor.Init();
			Item.Init();
			Iact.Init();
			Stage.Init();
			Entity.Init();
			Grid.Init();
			*/
		}

		static public void LoadAll() {
			Stopwatch watch = new Stopwatch();
			watch.Start();
			EditAll all;
			JsonSerializer ser = EditAll.Ser();
			TextAsset text = Resources.Load<TextAsset>("schema");
			using (StringReader sr = new StringReader(text.text))
			using (JsonReader r = new JsonTextReader(sr)) {
				all = ser.Deserialize<EditAll>(r);
			}
			Resources.UnloadAsset(text);
			watch.Stop();
			Debug.Log(string.Format("load schema json time {0}", watch.Elapsed));
			watch.Reset();

			watch.Start();
			Floor.AddAll(all.floors);
			Biome.AddAll(all.biomes);
			Entity.AddAll(all.boulders);
			Entity.AddAll(all.creatures);
			Entity.AddAll(all.trees);
			Entity.AddAll(all.workshops);
			Item.AddAll(all.items);
			watch.Stop();
			Debug.Log(string.Format("load schema data time {0}", watch.Elapsed));
		}
	}
}
