using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Schema {
	public class All {
		static public void Init() {
			Sprite.Init();
			Stage.Init();
			LoadAll();
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
			Biome.AddAll(all.biome);
			Entity.AddAll(all.boulder);
			Entity.AddAll(all.creature);
			Entity.AddAll(all.trees);
			Entity.AddAll(all.workshop);
			Floor.AddAll(all.floor);
			Iact.AddAll(all.move);
			Iact.AddAll(all.attack);
			Iact.AddAll(all.build);
			Iact.AddAll(all.make);
			Iact.AddAll(all.pick);
			Iact.AddAll(all.rest);
			Iact.AddAll(all.travel);
			Item.AddAll(all.item);
			watch.Stop();
			Debug.Log(string.Format("load schema data time {0}", watch.Elapsed));
		}
	}
}
