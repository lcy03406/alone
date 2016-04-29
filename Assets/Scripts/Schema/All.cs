using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Collections.Generic;

namespace Schema {
	public class All {
		static public void Init() {
			Sprite.Init();
			Stage.Init();
			Entity.Init();
			LoadAll();
			new All().LoadFiles();
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
			Entity.AddAll(all.tree);
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

		public interface RunData {
			int GetID();
		}

		public interface EditData {
			void LoadTo(All all);
		}

		public abstract class Data : EditData, RunData {
			public int ID;

			int RunData.GetID() {
				return ID;
			}

			void EditData.LoadTo(All all) {
				all.Add(this);
			}
		}

		public All all;

		private SortedList<int, RunData> data = new SortedList<int, RunData>();

		private All() {
			all = this;
		}

		public void Add(RunData d) {
			int id = d.GetID();
			if (id > 0) {
				data.Add(id, d);
			}
		}

		public void LoadFiles() {
			TextAsset[] texts = Resources.LoadAll<TextAsset>("csv");
			foreach (TextAsset ass in texts) {
				string name = ass.name.Split('#')[0];
				string typename = "Schema.Edit" + name;
                Type type = Type.GetType(typename);
				if (type == null || !type.IsSealed || !typeof(EditData).IsAssignableFrom(type)) {
					Debug.Log(string.Format("fail to load schema data {0} : no such type.", ass.name));
					continue;
				}
				try {
					using (StringReader reader = new StringReader(ass.text)) {
						Utility.Table.Load(reader, type, (obj) => { (obj as EditData).LoadTo(this); });
					}
				} catch (Exception e) {
					Debug.Log(string.Format("fail to load schema data {0} : {1}", ass.name, e));
				}
			}
			foreach (TextAsset ass in texts) {
				Resources.UnloadAsset(ass);
			}
		}
	}
	public sealed class EditDropTable : All.Data {
		public string Name;
		public string ItemName;
		public string ItemDes;
		public struct DropItem {
			public int ID;
			public int Num;
		}
		public List<DropItem> Item;
	}
}

