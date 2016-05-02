using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Collections.Generic;
using Utility;

namespace Schema {
	public class All : Table.Loader {
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

		public abstract class Data {
			public int ID { get; set; }

			public void LoadTo(All all) {
				all.Add(this);
			}
		}

		public static All all;

		private SortedList<int, Data> data = new SortedList<int, Data>();
		private HashSet<int> empty = new HashSet<int>();

		private All() {
			all = this;
		}

		public void Add(Data d) {
			int id = d.ID;
			if (id > 0) {
				data.Add(id, d);
			}
		}

		public Data Get(int id) {
			Data value = null;
			data.TryGetValue(id, out value);
			return value;
		}

		public Data Get(int id, Type type) {
			Data value = null;
			if (!data.TryGetValue(id, out value))
				return null;
			if (!type.IsAssignableFrom(value.GetType()))
				return null;
			return value;
		}

		public T Get<T>(int id) where T : Data {
			return (T)Get(id, typeof(T));
		}

		public Data GetOrAddEmpty(int id, Type type) {
			Data value = null;
			if (!data.TryGetValue(id, out value)) {
				value = (Data) Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				empty.Add(id);
				return value;
			}
			if (!type.IsAssignableFrom(value.GetType()))
				return null;
			return value;
		}

		public void LoadFiles() {
			TextAsset[] texts = Resources.LoadAll<TextAsset>("csv");
			foreach (TextAsset ass in texts) {
				string name = ass.name.Split('#')[0];
				string typename = "Schema.Edit" + name;
                Type type = Type.GetType(typename);
				if (type == null || !type.IsSealed || !typeof(Data).IsAssignableFrom(type)) {
					throw new InvalidDataException(string.Format("fail to load schema data {0} : no such type.", ass.name));
				}
				try {
					using (StringReader reader = new StringReader(ass.text)) {
						Table.Load(reader, type, this);
					}
				} catch (Exception e) {
					throw new InvalidDataException(string.Format("fail to load schema data {0}", ass.name), e);
				}
			}
			foreach (TextAsset ass in texts) {
				Resources.UnloadAsset(ass);
			}
		}

		object Table.Loader.Allocate(Type type, int id) {
			if (empty.Remove(id)) {
				Data value = Get(id);
				if (value.GetType() != type) {
					throw new InvalidDataException(string.Format("type mismatch empty:{0} table:{1}", value.GetType(), type));
				}
				return value;
			} else {
				Data value = (Data)Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				return value;
			}
		}
	}
	interface HasItemDes {
		string ItemDes { get; set; }
	}
	public sealed class EditDropTable : All.Data, HasItemDes {
		public string Name;
		public All.Data ItemName;
		public string ItemDes;
		public struct DropItem {
			public int ID;
			public int Num;
		}
		public List<DropItem> Item;

		string HasItemDes.ItemDes { get; set; }
	}
}

